using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;

namespace SampleGovernedActivities.Activities.Constraints
{
    class EmailRecord
    {
        public DateTime EmailCreated { get; set; }
        public int Count { get; set; }

        internal EmailRecord(int count)
        {
            this.EmailCreated = DateTime.Now;
            this.Count = count;
        }

        internal EmailRecord(int count, DateTime sent)
        {
            this.EmailCreated = sent;
            this.Count = count;
        }

        public EmailRecord()
        {

        }
    }

    class DailyEmailHistory
    {
        public const string FileName = "dailymailcounts.json";
        public static readonly string StorageDirectory = Environment.ExpandEnvironmentVariables("%localappdata%\\EYMail");
        public static readonly string StorageFile = Path.Combine(StorageDirectory, FileName);

        protected List<EmailRecord> Records { get; set; }
        internal static readonly object FileLock = new object();


        public DailyEmailHistory()
        {
            LoadRecordHistory();
        }

        public static List<EmailRecord> FilterRecordsToL24Hours(List<EmailRecord> records)
        {

            DateTime l24Hours = DateTime.Now - new TimeSpan(0, 24, 0, 0);
            List<EmailRecord> results = records.Where(record => record.EmailCreated >= l24Hours).ToList();
            return results;
        }

        public int GetRollingCount()
        {
            return GetRollingCount(this.Records);
        }

        protected int GetRollingCount(List<EmailRecord> records)
        {
            int count = records.Sum(r => r.Count);
            return count;
        }

        protected void LoadRecordHistory()
        {
            lock (FileLock)
            {
                try
                {
                    if (File.Exists(StorageFile))
                    {
                        using (StreamReader sr = new StreamReader(StorageFile))
                        {
                            string contents = sr.ReadToEnd();
                            List<EmailRecord> records = JsonConvert.DeserializeObject<List<EmailRecord>>(contents);
                            this.Records = FilterRecordsToL24Hours(records);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            if (this.Records == null)
            {
                this.Records = new List<EmailRecord>();
            }
        }

        public void NewMailSent(int recipientCount, bool persistChanges = true)
        {
            EmailRecord newRecord = new EmailRecord(recipientCount);
            AddMailRecord(newRecord, persistChanges);
        }

        internal void AddMailRecord(EmailRecord record, bool persistChanges)
        {
            this.Records.Add(record);
            if (persistChanges)
            {
                SaveRecordHistory();
            }
        }

        public void SaveRecordHistory()
        {
            lock (FileLock)
            {

                if (!Directory.Exists(DailyEmailHistory.StorageDirectory))
                {
                    Directory.CreateDirectory(StorageDirectory);
                }

                using (StreamWriter sw = new StreamWriter(DailyEmailHistory.StorageFile))
                {
                    string fileContents = JsonConvert.SerializeObject(this.Records);
                    sw.Write(fileContents);
                }
            }
        }

    }
}
