using System;
using System.Collections.Generic;
using WPF.Dto;
using WPF.Repository;

namespace WPF.Model
{
    internal class RecordModel
    {
        private readonly RecordRepository repo;

        public RecordModel()
        {
            repo = new RecordRepository();
        }

        public int GetNotReturnBookCount()
        {
            return repo.GetNotReturnBookCount(DateTime.Today);
        }

        public List<RecordDto> GetData() => repo.GetAllRecords();

        public bool SaveData(RecordDto record) => repo.SaveRecord(record);

        public bool SetReturnedStatus(RecordDto record) => repo.UpdateReturnStatus(record);

        public bool DeleteData(RecordDto record) => repo.DeleteRecord((int)record.Id);

        public List<RecordDto> GetSearchData(string keyword) => repo.SearchRecords(keyword);
    }
}
