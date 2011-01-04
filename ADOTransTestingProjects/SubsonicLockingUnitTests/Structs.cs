


using System;
using SubSonic.Schema;
using System.Collections.Generic;
using SubSonic.DataProviders;
using System.Data;

namespace SubsonicLockingUnitTests {
	
        /// <summary>
        /// Table: LockingTest
        /// Primary Key: LockingTestId
        /// </summary>

        public class LockingTestTable: DatabaseTable {
            
            public LockingTestTable(IDataProvider provider):base("LockingTest",provider){
                ClassName = "LockingTest";
                SchemaName = "dbo";
                

               
if(false)
{}else{
 Columns.Add(new DatabaseColumn("LockingTestId", this)
 {
      IsPrimaryKey = true,
      DataType = DbType.Int32,
      IsNullable = false,
      AutoIncrement = true,
      IsForeignKey = false,
      MaxLength = 0,
      
       //THIS LINE DO THE TRICK.
       IsReadOnly = false

 });
}
                
                

               
if(false)
{}else{
 Columns.Add(new DatabaseColumn("FirstName", this)
 {
      IsPrimaryKey = false,
      DataType = DbType.AnsiString,
      IsNullable = true,
      AutoIncrement = false,
      IsForeignKey = false,
      MaxLength = 100,
      
       //THIS LINE DO THE TRICK.
       IsReadOnly = false

 });
}
                
                

               
if(false)
{}else{
 Columns.Add(new DatabaseColumn("LastName", this)
 {
      IsPrimaryKey = false,
      DataType = DbType.AnsiString,
      IsNullable = true,
      AutoIncrement = false,
      IsForeignKey = false,
      MaxLength = 100,
      
       //THIS LINE DO THE TRICK.
       IsReadOnly = false

 });
}
                
                

               
if(false)
{}else{
 Columns.Add(new DatabaseColumn("Age", this)
 {
      IsPrimaryKey = false,
      DataType = DbType.Int32,
      IsNullable = true,
      AutoIncrement = false,
      IsForeignKey = false,
      MaxLength = 0,
      
       //THIS LINE DO THE TRICK.
       IsReadOnly = false

 });
}
                
                

               
if(false)
{}else{
 Columns.Add(new DatabaseColumn("RowVer", this)
 {
      IsPrimaryKey = false,
      DataType = DbType.Binary,
      IsNullable = false,
      AutoIncrement = false,
      IsForeignKey = false,
      MaxLength = 0,
      
       //THIS LINE DO THE TRICK.
       IsReadOnly = true

 });
}
                
                
                    
                
                
            }

            public IColumn LockingTestId{
                get{
                    return this.GetColumn("LockingTestId");
                }
            }
				
   			public static string LockingTestIdColumn{
			      get{
        			return "LockingTestId";
      			}
		    }
            
            public IColumn FirstName{
                get{
                    return this.GetColumn("FirstName");
                }
            }
				
   			public static string FirstNameColumn{
			      get{
        			return "FirstName";
      			}
		    }
            
            public IColumn LastName{
                get{
                    return this.GetColumn("LastName");
                }
            }
				
   			public static string LastNameColumn{
			      get{
        			return "LastName";
      			}
		    }
            
            public IColumn Age{
                get{
                    return this.GetColumn("Age");
                }
            }
				
   			public static string AgeColumn{
			      get{
        			return "Age";
      			}
		    }
            
            public IColumn RowVer{
                get{
                    return this.GetColumn("RowVer");
                }
            }
				
   			public static string RowVerColumn{
			      get{
        			return "RowVer";
      			}
		    }
            
                    
        }
        
}