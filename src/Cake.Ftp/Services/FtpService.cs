using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using FluentFTP;
using FluentFTP.Rules;

namespace Cake.Ftp.Services {
    /// <summary>
    /// The FTP Service.
    /// </summary>
    public class FtpService : IFtpService {
        private readonly ICakeLog _log;

        /// <summary>
        /// Intializes a new instance of the <see cref="FtpService"/> class. 
        /// </summary>
        /// <param name="log"></param>
        public FtpService(ICakeLog log) {
            _log = log;
        }

        /// <summary>
        /// Uploads a file.
        /// </summary>
        /// <param name="host">host of the FTP Client</param>
        /// <param name="remotePath">path on the file on the server</param>
        /// <param name="uploadFile">The file to upload.</param>
        /// <param name="settings">Ftp Settings</param>
        public void UploadFile(string host, string remotePath, IFile uploadFile, FtpSettings settings) {

            using (var client = CreateClient(host, settings)) {
                Connect(client, settings.AutoDetectConnectionSettings);   
                
                client.UploadFile(uploadFile.Path.FullPath, remotePath, Translate(settings.FileExistsBehavior), settings.CreateRemoteDirectory);
                client.Disconnect();
            }
        }

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="host">host of the FTP Client</param>
        /// <param name="remotePath">path on the file on the server</param>
        /// <param name="settings">Ftp Settings</param>
        public void DeleteFile(string host, string remotePath, FtpSettings settings) {

            using (var client = CreateClient(host, settings)) {
                Connect(client, settings.AutoDetectConnectionSettings);

                client.DeleteFile(remotePath);
                client.Disconnect();
            }

        }

        /// <summary>
        /// Upload and Overwite remote folder with local folder for default
        /// </summary>
        /// <param name="host"></param>
        /// <param name="remoteFolder"></param>
        /// <param name="localFolder"></param>
        /// <param name="settings"></param>
        /// <param name="ftpFolderSyncMode"></param>
        /// <param name="ftpRemoteExists"></param>
        /// <param name="ftpVerify"></param>
        /// <param name="rules"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        public List<FtpResult> UploadFolder(string host, string remoteFolder, string localFolder, FtpSettings settings,
            List<FtpRule> rules = null, Action<FtpProgress> process = null,
            FtpFolderSyncMode ftpFolderSyncMode  = FtpFolderSyncMode.Mirror, FtpRemoteExists ftpRemoteExists = FtpRemoteExists.Overwrite, FtpVerify ftpVerify = FtpVerify.None
            )
        {
            using (var client = CreateClient(host, settings))
            {
                Connect(client, settings.AutoDetectConnectionSettings);
                client.RetryAttempts = 3;
                _log.Information("Start upload folder: " + localFolder + " -> " + remoteFolder);
                var result = client.UploadDirectory(localFolder, remoteFolder, ftpFolderSyncMode, ftpRemoteExists, ftpVerify, rules, process);
                client.Disconnect();                
                return result;
            }
        }

        /// Uploads a file.
        /// </summary>
        /// <param name="host">host of the FTP Client</param>
        /// <param name="directories">Dictionary keyed by the remote path with a list of local files to upload to the remote path</param>
        /// <param name="settings">Ftp Settings</param>
        public void UploadDirectories(string host, Dictionary<string, IEnumerable<string>> directories, FtpSettings settings) {

            using (var client = CreateClient(host, settings))
            {
                Connect(client, settings.AutoDetectConnectionSettings);

                // Loop through each directory to upload files using client.UploadFiles which is more performant
                foreach (var localDirectory in directories) {
                    
                    client.UploadFiles(
                        localDirectory.Value,
                        localDirectory.Key,
                        Translate(settings.FileExistsBehavior),
                        settings.CreateRemoteDirectory,
                        errorHandling: FtpError.Throw);
                }

                client.Disconnect();
            }
        }


        private FluentFTP.FtpClient CreateClient(string host, FtpSettings settings) {
            var client = new FluentFTP.FtpClient(host, new NetworkCredential(settings.Username, settings.Password));
            client.OnLogEvent += OnLogEvent;

            client.ValidateAnyCertificate = settings.ValidateAnyCertificate;
            
            if (settings.AutoDetectConnectionSettings) 
                return client;

            client.EncryptionMode = Translate(settings.EncryptionMode);
            client.SslProtocols = settings.SslProtocols;
            client.DataConnectionType = Translate(settings.DataConnectionType);

            return client;
        }

        private void Connect(FluentFTP.FtpClient client, bool autoDetectConnectionSettings) {
            if (autoDetectConnectionSettings) {
                client.AutoConnect();
            }
            else {
                client.Connect();
            }
        }

        private void OnLogEvent(FtpTraceLevel level, string message) {
            switch (level) {
                case FtpTraceLevel.Error: {
                    _log.Error(message);
                    break;
                }
                //case FtpTraceLevel.Warn: {
                //    _log.Warning(message);
                //    break;
                //}
                //case FtpTraceLevel.Info: {
                //    _log.Information(message);
                //    break;
                //}
                //case FtpTraceLevel.Verbose: {
                //    _log.Verbose(message);
                //    break;
                //}
            }
        }

        private FtpRemoteExists Translate(FtpExists ftpExists) {
            switch (ftpExists) {
                case FtpExists.Append:
                    return FtpRemoteExists.Append;
                case FtpExists.AppendNoCheck:
                    return FtpRemoteExists.AppendNoCheck;
                case FtpExists.NoCheck :
                    return FtpRemoteExists.NoCheck;
                case FtpExists.Overwrite:
                    return FtpRemoteExists.Overwrite;
                case FtpExists.Skip:
                    return FtpRemoteExists.Skip;
            }

            throw new InvalidEnumArgumentException($"{nameof(FtpExists)} enum value {ftpExists} is invalid as it has not been mapped");
        }

        private FluentFTP.FtpEncryptionMode Translate(FtpEncryptionMode ftpEncryptionMode)
        {
            switch (ftpEncryptionMode)
            {
                case FtpEncryptionMode.Explicit:
                    return FluentFTP.FtpEncryptionMode.Explicit;
                case FtpEncryptionMode.Implicit:
                    return FluentFTP.FtpEncryptionMode.Implicit;
                case FtpEncryptionMode.None:
                    return FluentFTP.FtpEncryptionMode.None;
            }

            throw new InvalidEnumArgumentException($"{nameof(FtpEncryptionMode)} enum value {ftpEncryptionMode} is invalid as it has not been mapped");
        }

        private FluentFTP.FtpDataConnectionType Translate(FtpDataConnectionType ftpDataConnectionType) {
            switch (ftpDataConnectionType) {
                case FtpDataConnectionType.AutoActive:
                    return FluentFTP.FtpDataConnectionType.AutoActive;
                case FtpDataConnectionType.AutoPassive:
                    return FluentFTP.FtpDataConnectionType.AutoPassive;
                case FtpDataConnectionType.EPRT:
                    return FluentFTP.FtpDataConnectionType.EPRT;
                case FtpDataConnectionType.EPSV:
                    return FluentFTP.FtpDataConnectionType.EPSV;
                case FtpDataConnectionType.PASV:
                    return FluentFTP.FtpDataConnectionType.PASV;
                case FtpDataConnectionType.PASVEX:
                    return FluentFTP.FtpDataConnectionType.PASVEX;
                case FtpDataConnectionType.PORT:
                    return FluentFTP.FtpDataConnectionType.PORT;
            }
            throw new InvalidEnumArgumentException($"{nameof(FtpDataConnectionType)} enum value {ftpDataConnectionType} is invalid as it has not been mapped");
        }

        /// <summary>
        /// Parallel upload folder
        /// </summary>
        /// <param name="host"></param>
        /// <param name="sourcePath"></param>
        /// <param name="remotePath"></param>
        /// <param name="settings"></param>
        /// <param name="parallel"></param>
        /// <param name="ignoreRule">Ignore rules.-i: relativePath</param>
        public void UploadFolderParallel(string host, string remotePath, string sourcePath, FtpSettings settings, int parallel = 5, Func<string, bool> ignoreRule = null)
        {
            if (parallel < 1) parallel = 5;
            try
            {
                var cancellationToken = new CancellationTokenSource();
                var sourceFolder = new DirectoryInfo(sourcePath);
                var fileSystemInfos = sourceFolder.EnumerateFileSystemInfos("*", SearchOption.AllDirectories);                                
                var _taskScheduler = new LimitedConcurrencyLevelTaskScheduler(parallel);
                var taskList = new List<Task>();                

                foreach (var fileInfo in fileSystemInfos)
                {
                    if (fileInfo.FullName == sourceFolder.FullName) { continue; }
                    var currentFile = fileInfo; //It's because the task shows the value of i at the time that the task is executed, not when the task was created.
                    var relativePath = Uri.UnescapeDataString(currentFile.FullName.Remove(0, sourceFolder.FullName.Length));                    

                    if (ignoreRule != null && ignoreRule(relativePath)) { continue; }

                    var remoteRelativePath = string.Concat(remotePath, relativePath.Replace("\\", "/"));

                    Task task = Task.Factory.StartNew(() =>
                    {                        
                        try
                        {
                            using (var client = CreateClient(host, settings))
                            {
                                Connect(client, settings.AutoDetectConnectionSettings);
                                if (currentFile.Attributes == FileAttributes.Directory)
                                {
                                    //Create if the dir doesn't exist
                                    if (!client.DirectoryExists(remoteRelativePath))
                                    {
                                        if(client.CreateDirectory(remoteRelativePath, true))
                                        {
                                            _log.Information("Created folder: " + remoteRelativePath);
                                        }
                                    }                                    
                                }
                                else
                                {                                                                        
                                    client.TransferChunkSize = 10000;
                                    client.UploadDataType = FtpDataType.Binary;
                                    client.RetryAttempts = 3;

                                    //only upload newer file or different size - untest
                                    //var remoteFile = client.GetObjectInfo(remotePath);
                                    //if (remoteFile != null)
                                    //{
                                    //    if(currentFile.CreationTime <= remoteFile.Modified && ((FileInfo)currentFile).Length == remoteFile.Size)
                                    //    {
                                    //        _log.Information(DateTime.Now + " Skiped file: " + currentFile.FullName);
                                    //        client.Disconnect();
                                    //        return;
                                    //    }                                        
                                    //}
                                    
                                    var fileUploadedResult = client.UploadFile(currentFile.FullName, remoteRelativePath, FtpRemoteExists.Overwrite, true, FtpVerify.Retry | FtpVerify.Throw);
                                    if (fileUploadedResult.IsSuccess())
                                    {
                                        _log.Information(currentFile.FullName + " -> " + remoteRelativePath);
                                    }
                                    else
                                    {
                                        _log.Warning(DateTime.Now + " Failure file: " + currentFile.FullName);
                                    }                                    
                                }
                                client.Disconnect();
                            }

                        }
                        catch (Exception ex)
                        {
                            _log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) +
                                            "Error in Uploading file " + remoteRelativePath + " " + ex.Message +
                                            Environment.NewLine + ex.InnerException + Environment.NewLine +
                                            ex.StackTrace);
                        }                        
                    }, cancellationToken.Token, TaskCreationOptions.AttachedToParent, _taskScheduler);
                    taskList.Add(task);                    
                }
                Task.WaitAll(taskList.ToArray(), cancellationToken.Token);

                _log.Information(DateTime.Now + " Upload done");
            }
            catch (Exception ex)
            {
                _log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + "Error in Uploading file: " + sourcePath + " " + ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="remotePath"></param>
        /// <param name="localPath"></param>
        /// <param name="settings"></param>
        public void DownloadFile(string host, string remotePath, string localPath, FtpSettings settings)
        {
            using (var client = CreateClient(host, settings))
            {
                Connect(client, settings.AutoDetectConnectionSettings);
                client.RetryAttempts = 3;
                client.DownloadFile(localPath, remotePath, FtpLocalExists.Overwrite, FtpVerify.Retry | FtpVerify.Throw);
                client.Disconnect();
            }
        }

    }

    /// <summary>
    /// Provides a task scheduler that ensures a maximum concurrency level while running on top of the thread pool.
    /// https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskscheduler?view=netframework-4.8
    /// </summary>
    public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
    {
        // Indicates whether the current thread is processing work items.
        [ThreadStatic]
        private static bool _currentThreadIsProcessingItems;

        // The list of tasks to be executed 
        private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks)

        // The maximum concurrency level allowed by this scheduler. 
        private readonly int _maxDegreeOfParallelism;

        // Indicates whether the scheduler is currently processing work items. 
        private int _delegatesQueuedOrRunning = 0;

        /// <summary>
        /// Creates a new instance with the specified degree of parallelism. 
        /// </summary>
        /// <param name="maxDegreeOfParallelism"></param>
        public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
        {
            if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        /// <summary>
        /// Queues a task to the scheduler.
        /// </summary>
        /// <param name="task"></param>
        protected sealed override void QueueTask(Task task)
        {
            // Add the task to the list of tasks to be processed.  If there aren't enough 
            // delegates currently queued or running to process tasks, schedule another. 
            lock (_tasks)
            {
                _tasks.AddLast(task);
                if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
                {
                    ++_delegatesQueuedOrRunning;
                    NotifyThreadPoolOfPendingWork();
                }
            }
        }

        /// <summary>
        /// Inform the ThreadPool that there's work to be executed for this scheduler. 
        /// </summary>
        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.UnsafeQueueUserWorkItem(_ =>
            {
                // Note that the current thread is now processing work items.
                // This is necessary to enable inlining of tasks into this thread.
                _currentThreadIsProcessingItems = true;
                try
                {
                    // Process all available items in the queue.
                    while (true)
                    {
                        Task item;
                        lock (_tasks)
                        {
                            // When there are no more items to be processed,
                            // note that we're done processing, and get out.
                            if (_tasks.Count == 0)
                            {
                                --_delegatesQueuedOrRunning;
                                break;
                            }

                            // Get the next item from the queue
                            item = _tasks.First.Value;
                            _tasks.RemoveFirst();
                        }

                        // Execute the task we pulled out of the queue
                        base.TryExecuteTask(item);
                    }
                }
                // We're done processing items on the current thread
                finally { _currentThreadIsProcessingItems = false; }
            }, null);
        }

        /// <summary>
        /// Attempts to execute the specified task on the current thread. 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="taskWasPreviouslyQueued"></param>
        /// <returns></returns>
        protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            // If this thread isn't already processing a task, we don't support inlining
            if (!_currentThreadIsProcessingItems) return false;

            // If the task was previously queued, remove it from the queue
            if (taskWasPreviouslyQueued)
                // Try to run the task. 
                if (TryDequeue(task))
                    return base.TryExecuteTask(task);
                else
                    return false;
            else
                return base.TryExecuteTask(task);
        }

        /// <summary>
        /// Attempt to remove a previously scheduled task from the scheduler. 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        protected sealed override bool TryDequeue(Task task)
        {
            lock (_tasks) return _tasks.Remove(task);
        }

        /// <summary>
        /// Gets the maximum concurrency level supported by this scheduler. 
        /// </summary>
        public sealed override int MaximumConcurrencyLevel { get { return _maxDegreeOfParallelism; } }

        /// <summary>
        /// Gets an enumerable of the tasks currently scheduled on this scheduler. 
        /// </summary>
        /// <returns></returns>
        protected sealed override IEnumerable<Task> GetScheduledTasks()
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(_tasks, ref lockTaken);
                if (lockTaken) return _tasks;
                else throw new NotSupportedException();
            }
            finally
            {
                if (lockTaken) Monitor.Exit(_tasks);
            }
        }
    }
}
