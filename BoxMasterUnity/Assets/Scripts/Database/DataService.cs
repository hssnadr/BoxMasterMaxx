using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CRI.HitBox.Database
{
    public static class DataService
    {
        /// <summary>
        /// The result of an insertion request.
        /// </summary>
        public struct InsertionResult<T> where T: DataEntry
        {
            /// <summary>
            /// Was the insertion successful ?
            /// </summary>
            public bool successful;
            /// <summary>
            /// The id of the insert if the table had an auto_increment field.
            /// </summary>
            public int id;
            /// <summary>
            /// The inserted data.
            /// </summary>
            public T data;

            public InsertionResult(bool successful, int id, T data)
            {
                this.successful = successful;
                this.id = id;
                this.data = data;
            }

            public override string ToString()
            {
                return string.Format("InsertionResult = [successful = {0}, id = {1}, data = {2}]", successful, id, data);
            }
        }

        /// <summary>
        /// The result of an update request.
        /// </summary>
        public struct UpdateResult<T> where T : DataEntry
        {
            /// <summary>
            /// Was the update successful ?
            /// </summary>
            public bool successful;
            /// <summary>
            /// The update data.
            /// </summary>
            public T data;

            public UpdateResult(bool successful, T data)
            {
                this.successful = successful;
                this.data = data;
            }

            public override string ToString()
            {
                return string.Format("UpdateResult = [successful = {0}, data = {1}]", successful, data);
            }
        }

        /// <summary>
        /// The result of an selection request.
        /// </summary>
        public struct SelectResult<T> where T : DataEntry
        {
            /// <summary>
            /// Was the selection successful ?
            /// </summary>
            public bool successful;
            /// <summary>
            /// The selected dataList.
            /// </summary>
            public List<T> dataList;
            /// <summary>
            /// Number of lines selected.
            /// </summary>
            public int count
            {
                get
                {
                    return dataList.Count;
                }
            }

            public SelectResult(bool successful, List<T> dataList)
            {
                this.successful = successful;
                this.dataList = dataList;
            }

            public override string ToString()
            {
                return string.Format("SelectResult = [successful = {0}, data = {1}, count = {2}]", successful, dataList, count);
            }
        }

        public const string databaseName = "hitbox";        
        /// <summary>
        /// Loads a list of DataEntry from the database.
        /// </summary>
        /// <typeparam name="T">A non abstract type of DataEntry</typeparam>
        /// <returns>A list of DataEntry</returns>
        public static async Task<SelectResult<T>> LoadData<T>() where T : DataEntry, new()
        {
            try
            {
                var dataEntry = new T();
                WWW www = await new WWW(string.Format("http://localhost/{0}/select_all_{1}.php", databaseName, dataEntry.GetTableName()));
                if (!string.IsNullOrEmpty(www.error))
                {
                    throw new Exception(www.error);
                }
                if (!string.IsNullOrEmpty(www.text))
                {
                    if (www.text.ToUpper().Contains("ERROR"))
                        throw new Exception(www.text);
                }
                string dataString = www.text;
                return new SelectResult<T>(true, DataEntry.ToDataEntryList<T>(dataString));
            }
            catch (Exception e)
            {
                Debug.LogError("Error for LoadData of type " + new T().GetType() + " :" + e.Message);
                return new SelectResult<T>(false, null);
            }
        }

        /// <summary>
        /// Loads a list of DataEntry from the database and applies an action to the list when the list finished loading.
        /// </summary>
        /// <typeparam name="T">A non abstract type of DataEntry</typeparam>
        /// <param name="callback">The action that will be called on the list when it finishes loading.</param>
        public static async void LoadData<T>(Action<SelectResult<T>> callback) where T : DataEntry, new()
        {
            var result = await LoadData<T>();
            callback(result);
        }

        /// <summary>
        /// Inserts a DataEntry into a database.
        /// </summary>
        /// <param name="dataEntry">A data entry.</param>
        /// <returns>An instance of InsertionResult.</returns>
        public static async Task<InsertionResult<T>> InsertData<T>(T dataEntry) where T : DataEntry
        {
            try
            {
                WWW www = await new WWW(string.Format("http://localhost/{0}/insert_{1}.php", databaseName, dataEntry.GetTypeName()), dataEntry.GetForm());
                int id = 0;
                if (!string.IsNullOrEmpty(www.error))
                {
                    throw new Exception(www.error);
                }
                if (!string.IsNullOrEmpty(www.text))
                {
                    if (www.text.ToUpper().Contains("ERROR"))
                        throw new Exception(www.text);
                    int.TryParse(www.text, out id);
                }
                if (dataEntry.HasAutoIncrementPrimaryKey())
                    dataEntry.SetAutoIncrementPrimaryKey(id);
                return new InsertionResult<T>(true, id, dataEntry); 
            }
            catch (Exception e)
            {
                Debug.LogError("Error during insertion of data entry " + dataEntry + ": " + e.Message);
                return new InsertionResult<T>(false, 0, dataEntry);
            }
        }

        /// <summary>
        /// Inserts a DataEntry into a database and applies an action to the resulting InsertionResult.
        /// </summary>
        /// <param name="dataEntry">A data entry.</param>
        /// <param name="callback">The action that will be called when the insertion ends.</param>
        public static async void InsertData<T>(T dataEntry, Action<InsertionResult<T>> callback) where T : DataEntry
        {
            var result = await InsertData(dataEntry);
            callback(result);
        }

        /// <summary>
        /// Update a DataEntry of a database.
        /// </summary>
        /// <param name="dataEntry">A data entry.</param>
        /// <returns>An instance of UpdateResult</returns>
        public static async Task<UpdateResult<T>> UpdateData<T>(T dataEntry) where T : DataEntry
        {
            try
            {
                WWW www = await new WWW(string.Format("http://localhost/{0}/update_{1}.php", databaseName, dataEntry.GetTypeName()), dataEntry.GetForm());
                if (!string.IsNullOrEmpty(www.error))
                {
                    throw new Exception(www.error);
                }
                if (!string.IsNullOrEmpty(www.text))
                {
                    if (www.text.ToUpper().Contains("ERROR"))
                        throw new Exception(www.text);
                }
                return new UpdateResult<T>(true, dataEntry);
            }
            catch (Exception e)
            {
                Debug.LogError("Error during update of data entry " + dataEntry + ": " + e.Message);
                return new UpdateResult<T>(false, dataEntry);
            }
        }

        /// <summary>
        /// Update a DataEntry of a database and applies an action to the resulting UpdateResult.
        /// </summary>
        /// <param name="dataEntry">A data entry.</param>
        /// <param name="callback">The action that will be called when the update ends.</param>
        public static async void UpdateData<T>(T dataEntry, Action<UpdateResult<T>> callback) where T : DataEntry
        {
            var result = await UpdateData(dataEntry);
            callback(result);
        }
    }
}
