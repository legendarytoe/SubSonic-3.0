


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic.DataProviders;
using SubSonic.Extensions;
using System.Linq.Expressions;
using SubSonic.Schema;
using System.Collections;
using SubSonic;
using SubSonic.Repository;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;

namespace SubsonicLockingUnitTests
{
    
    
    /// <summary>
    /// A class which represents the LockingTest table in the SSLockingTestDb Database.
    /// </summary>
    public partial class LockingTest: IActiveRecord
    {
    
        #region Built-in testing
        static TestRepository<LockingTest> _testRepo;
        

        
        static void SetTestRepo(){
            _testRepo = _testRepo ?? new TestRepository<LockingTest>(new SubsonicLockingUnitTests.SSLockingTestDbDB());
        }
        public static void ResetTestRepo(){
            _testRepo = null;
            SetTestRepo();
        }
        public static void Setup(List<LockingTest> testlist){
            SetTestRepo();
            foreach (var item in testlist)
            {
                _testRepo._items.Add(item);
            }
        }
        public static void Setup(LockingTest item) {
            SetTestRepo();
            _testRepo._items.Add(item);
        }
        public static void Setup(int testItems) {
            SetTestRepo();
            for(int i=0;i<testItems;i++){
                LockingTest item=new LockingTest();
                _testRepo._items.Add(item);
            }
        }
        
        public bool TestMode = false;


        #endregion

        IRepository<LockingTest> _repo;
        ITable tbl;
        bool _isNew;
        public bool IsNew(){
            return _isNew;
        }
        
        public void SetIsLoaded(bool isLoaded){
            _isLoaded=isLoaded;
            if(isLoaded)
                OnLoaded();
        }
        
        public void SetIsNew(bool isNew){
            _isNew=isNew;
        }
        bool _isLoaded;
        public bool IsLoaded(){
            return _isLoaded;
        }
                
        List<IColumn> _dirtyColumns;
        public bool IsDirty(){
            return _dirtyColumns.Count>0;
        }
        
        public List<IColumn> GetDirtyColumns (){
            return _dirtyColumns;
        }

        SubsonicLockingUnitTests.SSLockingTestDbDB _db;
        public LockingTest(string connectionString, string providerName) {

            _db=new SubsonicLockingUnitTests.SSLockingTestDbDB(connectionString, providerName);
            Init();            
         }
        void Init(){
            TestMode=this._db.DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            _dirtyColumns=new List<IColumn>();
            if(TestMode){
                LockingTest.SetTestRepo();
                _repo=_testRepo;
            }else{
                _repo = new SubSonicRepository<LockingTest>(_db);
            }
            tbl=_repo.GetTable();
            SetIsNew(true);
            OnCreated();       

        }
        
        public LockingTest(){
             _db=new SubsonicLockingUnitTests.SSLockingTestDbDB();
            Init();            
        }
        
       
        partial void OnCreated();
            
        partial void OnLoaded();
        
        partial void OnSaved();
        
        partial void OnChanged();
        
        public IList<IColumn> Columns{
            get{
                return tbl.Columns;
            }
        }

        public LockingTest(Expression<Func<LockingTest, bool>> expression):this() {

            SetIsLoaded(_repo.Load(this,expression));
        }
        
       
        
        internal static IRepository<LockingTest> GetRepo(string connectionString, string providerName){
            SubsonicLockingUnitTests.SSLockingTestDbDB db;
            if(String.IsNullOrEmpty(connectionString)){
                db=new SubsonicLockingUnitTests.SSLockingTestDbDB();
            }else{
                db=new SubsonicLockingUnitTests.SSLockingTestDbDB(connectionString, providerName);
            }
            IRepository<LockingTest> _repo;
            
            if(db.TestMode){
                LockingTest.SetTestRepo();
                _repo=_testRepo;
            }else{
                _repo = new SubSonicRepository<LockingTest>(db);
            }
            return _repo;        
        }       
        
        internal static IRepository<LockingTest> GetRepo(){
            return GetRepo("","");
        }
        
        public static LockingTest SingleOrDefault(Expression<Func<LockingTest, bool>> expression) {

            var repo = GetRepo();
            var results=repo.Find(expression);
            LockingTest single=null;
            if(results.Count() > 0){
                single=results.ToList()[0];
                single.OnLoaded();
                single.SetIsLoaded(true);
                single.SetIsNew(false);
            }

            return single;
        }      
        
        public static LockingTest SingleOrDefault(Expression<Func<LockingTest, bool>> expression,string connectionString, string providerName) {
            var repo = GetRepo(connectionString,providerName);
            var results=repo.Find(expression);
            LockingTest single=null;
            if(results.Count() > 0){
                single=results.ToList()[0];
            }

            return single;


        }
        
        
        public static bool Exists(Expression<Func<LockingTest, bool>> expression,string connectionString, string providerName) {
           
            return All(connectionString,providerName).Any(expression);
        }        
        public static bool Exists(Expression<Func<LockingTest, bool>> expression) {
           
            return All().Any(expression);
        }        

        public static IList<LockingTest> Find(Expression<Func<LockingTest, bool>> expression) {
            
            var repo = GetRepo();
            return repo.Find(expression).ToList();
        }
        
        public static IList<LockingTest> Find(Expression<Func<LockingTest, bool>> expression,string connectionString, string providerName) {

            var repo = GetRepo(connectionString,providerName);
            return repo.Find(expression).ToList();

        }
        public static IQueryable<LockingTest> All(string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetAll();
        }
        public static IQueryable<LockingTest> All() {
            return GetRepo().GetAll();
        }
        
        public static PagedList<LockingTest> GetPaged(string sortBy, int pageIndex, int pageSize,string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetPaged(sortBy, pageIndex, pageSize);
        }
      
        public static PagedList<LockingTest> GetPaged(string sortBy, int pageIndex, int pageSize) {
            return GetRepo().GetPaged(sortBy, pageIndex, pageSize);
        }

        public static PagedList<LockingTest> GetPaged(int pageIndex, int pageSize,string connectionString, string providerName) {
            return GetRepo(connectionString,providerName).GetPaged(pageIndex, pageSize);
            
        }


        public static PagedList<LockingTest> GetPaged(int pageIndex, int pageSize) {
            return GetRepo().GetPaged(pageIndex, pageSize);
            
        }

        public string KeyName()
        {
            return "LockingTestId";
        }

        public object KeyValue()
        {
            return this.LockingTestId;
        }
        
        public void SetKeyValue(object value) {
            if (value != null && value!=DBNull.Value) {
                var settable = value.ChangeTypeTo<int>();
                this.GetType().GetProperty(this.KeyName()).SetValue(this, settable, null);
            }
        }
        
        public override string ToString(){
                            return this.FirstName.ToString();
                    }

        public override bool Equals(object obj){
            if(obj.GetType()==typeof(LockingTest)){
                LockingTest compare=(LockingTest)obj;
                return compare.KeyValue()==this.KeyValue();
            }else{
                return base.Equals(obj);
            }
        }

        
        public override int GetHashCode() {
            return this.LockingTestId;
        }
        
        public string DescriptorValue()
        {
                            return this.FirstName.ToString();
                    }

        public string DescriptorColumn() {
            return "FirstName";
        }
        public static string GetKeyColumn()
        {
            return "LockingTestId";
        }        
        public static string GetDescriptorColumn()
        {
            return "FirstName";
        }
        
        #region ' Foreign Keys '
        #endregion
        

        int _LockingTestId;
        public int LockingTestId
        {
            get { return _LockingTestId; }
            set
            {
                if(_LockingTestId!=value){
                    _LockingTestId=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="LockingTestId");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                if(_FirstName!=value){
                    _FirstName=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="FirstName");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set
            {
                if(_LastName!=value){
                    _LastName=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="LastName");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        int? _Age;
        public int? Age
        {
            get { return _Age; }
            set
            {
                if(_Age!=value){
                    _Age=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="Age");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }

        byte[] _RowVer;
        public byte[] RowVer
        {
            get { return _RowVer; }
            set
            {
                if(_RowVer!=value){
                    _RowVer=value;
                    var col=tbl.Columns.SingleOrDefault(x=>x.Name=="RowVer");
                    if(col!=null){
                        if(!_dirtyColumns.Any(x=>x.Name==col.Name) && _isLoaded){
                            _dirtyColumns.Add(col);
                        }
                    }
                    OnChanged();
                }
            }
        }



        public DbCommand GetUpdateCommand() {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToUpdateQuery(_db.Provider).GetCommand().ToDbCommand();
            
        }
        public DbCommand GetInsertCommand() {
 
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToInsertQuery(_db.Provider).GetCommand().ToDbCommand();
        }
        
        public DbCommand GetDeleteCommand() {
            if(TestMode)
                return _db.DataProvider.CreateCommand();
            else
                return this.ToDeleteQuery(_db.Provider).GetCommand().ToDbCommand();
        }
       
        
        public void Update(){
            Update(_db.DataProvider);
        }
        
        public void Update(IDataProvider provider){
        
            
                 
                int rowsEffected = 0;

                var cmd = this.GetUpdateCommand();

                cmd.CommandText = cmd.CommandText + " and RowVer = @RowVer";
                cmd.Parameters.Add(new SqlParameter("RowVer", this.RowVer));

                rowsEffected = cmd.ExecuteNonQuery();

                if (rowsEffected == 0)
                {
                    throw new ApplicationException("Optimistic concurrency logic has blocked this update.");
                }          
            
                //_repo.Update(this,provider);
			                _dirtyColumns.Clear();    
            OnSaved();
       }
 
        public void Add(){
            Add(_db.DataProvider);
        }
        
        
       
        public void Add(IDataProvider provider){

            
            var key=KeyValue();
            if(key==null){
                var newKey=_repo.Add(this,provider);
                this.SetKeyValue(newKey);
            }else{
                _repo.Add(this,provider);
            }
            SetIsNew(false);
            OnSaved();
        }
        
                
        
        public void Save() {
            Save(_db.DataProvider);
        }      
        public void Save(IDataProvider provider) {
            
           
            if (_isNew) {
                Add(provider);
                
            } else {
                Update(provider);
            }
            
        }

        

        public void Delete(IDataProvider provider) {
                   
                 
            _repo.Delete(KeyValue());
            
                    }


        public void Delete() {
            Delete(_db.DataProvider);
        }


        public static void Delete(Expression<Func<LockingTest, bool>> expression) {
            var repo = GetRepo();
            
       
            
            repo.DeleteMany(expression);
            
        }

        

        public void Load(IDataReader rdr) {
            Load(rdr, true);
        }
        public void Load(IDataReader rdr, bool closeReader) {
            if (rdr.Read()) {

                try {
                    rdr.Load(this);
                    SetIsNew(false);
                    SetIsLoaded(true);
                } catch {
                    SetIsLoaded(false);
                    throw;
                }
            }else{
                SetIsLoaded(false);
            }

            if (closeReader)
                rdr.Dispose();
        }
        

    } 
}
