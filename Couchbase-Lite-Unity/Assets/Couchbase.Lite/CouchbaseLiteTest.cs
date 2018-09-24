using System;
using System.IO;
using Couchbase.Lite;
using Couchbase.Lite.Logging;
using Couchbase.Lite.Query;
using Couchbase.Lite.Sync;
using UnityEngine;
using UnityEngine.UI;

public class CouchbaseLiteTest : MonoBehaviour {
    static CouchbaseLiteTest instance;

#pragma warning disable 0414,0649
    [SerializeField] string syncUrl = "ws://<server>:4984/<bucket>";

    [SerializeField] string databaseName = "<dbname>";

    [SerializeField] string username = "<username>";
    [SerializeField] string password = "<password>";

    [SerializeField] Text statusLabel;

    Database database;
    Replicator replicator;

    bool syncing;
    bool doneSyncing;
    bool stopReplicator;

    bool quitting = false, allDisposed = false;
#pragma warning restore 0414,0649

    void Awake()
    {
        if(syncUrl.Equals("ws://<server>:4984/<bucket>")) Debug.LogWarning("Sync URL has not been set!");
        if(databaseName.Equals("<dbname>")) Debug.LogWarning("Database name has not been set!");

        Debug.Assert(instance == null, "Multiple Couchbase instances not supported!");
        instance = this;

        Couchbase.Lite.Support.Unity.Activate();
    }

    void Start()
    {
        Database.SetLogLevel(LogDomain.All, LogLevel.Verbose);

        // Get the database (and create it if it doesn't exist)
        database = new Database(databaseName, new DatabaseConfiguration() { Directory = Path.Combine(Application.persistentDataPath, "CouchBase") });

        // Create a new document (i.e. a record) in the database
        string id = null;
        using(MutableDocument mutableDoc = new MutableDocument("meta")) {
            mutableDoc.SetFloat("version", 2.0f)
                .SetString("type", "SDK");

            // Save it to the database
            database.Save(mutableDoc);
            id = mutableDoc.Id;
        }

        statusLabel.text += "\n- Saved meta doc";

        // Update a document
        using(Document doc = database.GetDocument("meta"))
        using(MutableDocument mutableDoc = doc.ToMutable()) {
            mutableDoc.SetFloat("version", 2.0f);
            database.Save(mutableDoc);

            using(Document docAgain = database.GetDocument("meta")) {
                Debug.Log($"Document ID :: {docAgain.Id}");
                Debug.Log($"Version {docAgain.GetFloat("version")}");
                statusLabel.text += $"\n- Retrieved: {docAgain.Id}";
            }
        }

        // Create a query to fetch documents of type SDK
        // i.e. SELECT * FROM database WHERE type = "SDK"
        using(IWhere query = QueryBuilder.Select(SelectResult.All())
            .From(DataSource.Database(database))
            .Where(Expression.Property("version").EqualTo(Expression.Float(2.0f)))) {
            // Run the query
            IResultSet result = query.Execute();
            int count = result.AllResults().Count;
            Debug.Log($"Number of rows :: {count}");
            statusLabel.text += $"\n- Query result: {count}";
        }

#if UNITY_EDITOR
        Debug.LogWarning("CouchBase: In Unity Editor – Not replicating!");
        return;
#endif
        // Create replicator to push and pull changes to and from the cloud
        // NOTE: if Continuous is set to true, make sure to stop it and to clean up everything before exiting the application!
        URLEndpoint targetEndpoint = new URLEndpoint(new Uri(syncUrl));
        ReplicatorConfiguration replConfig = new ReplicatorConfiguration(database, targetEndpoint) {
            // Add authentication
            Authenticator = new BasicAuthenticator(username, password),
            ReplicatorType = ReplicatorType.PushAndPull,
            Continuous = false
        };

        // Create replicator
        replicator = new Replicator(replConfig);
        replicator.AddChangeListener((sender, args) => {
            if(args.Status.Error != null) {
            Debug.Log($"Error :: {args.Status.Error}");
            } else {
                if(replicator.Status.Activity != ReplicatorActivityLevel.Busy && replicator.Status.Activity != ReplicatorActivityLevel.Connecting) {
                    syncing = false;
                    Debug.Log($"Done syncing: {replicator.Status.Activity}");
                    doneSyncing = true;
                } else {
                    Debug.Log($"Progress: {args.Status.Progress.Completed} / {args.Status.Progress.Total}");
                }
            }
        });

        Debug.Log("Starting syncing...");
        statusLabel.text += "\n- Starting syncing...";
        replicator.Start();

        syncing = true;
        // Later, stop and dispose the replicator *before* closing/disposing the database
    }

    void FixedUpdate()
    {
        // NOTE: This quitting process is a first draft and has not been tested yet!
        // Please make sure you test your own process before using in production.
        if(quitting) {
            if(replicator != null && replicator.Status.Activity != ReplicatorActivityLevel.Stopped) {
                try {
                    replicator.Stop();
                } catch(Exception) {}
            }
            else {
                if(replicator != null) replicator.Dispose();

                try {
                    database.GetDocument("meta");
                    database.Close();
                    database.Dispose();
                } catch(ObjectDisposedException) {
                    allDisposed = true;
                    Application.Quit();
                }
            }
        }
        else if(!syncing) {
            if(doneSyncing) {
                statusLabel.text += $"\n- Done syncing: {replicator.Status.Activity}";
                doneSyncing = false;
            }
            // Other updates
        }
    }

    static bool WantsToQuit()
    {
        if(instance.quitting) Debug.LogWarning("Force quitting!");

        if(instance.allDisposed || instance.quitting) {
            return true;
        } else {
            instance.quitting = true;
            return false;
        }
    }

    [RuntimeInitializeOnLoadMethod]
    static void RunOnStart()
    {
        Application.wantsToQuit += WantsToQuit;
    }
}
