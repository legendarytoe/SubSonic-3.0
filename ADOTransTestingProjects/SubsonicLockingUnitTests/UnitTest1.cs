using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubSonic.DataProviders;

namespace SubsonicLockingUnitTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {

            try
            {

                List<LockingTest> allRecs = LockingTest.All().ToList();

                allRecs.ForEach(a => a.Delete());

                LockingTest ltRec1 = new LockingTest();
                ltRec1.FirstName = "Bruce";
                ltRec1.LastName = "Wolf";
                ltRec1.Age = 58;
                ///ltRec1.RowVer = null;

                ltRec1.Save();

                LockingTest ltRec2 = new LockingTest();
                ltRec2.FirstName = "Dan";
                ltRec2.LastName = "Profft";
                ltRec2.Age = 39;
                ///ltRec2.RowVer = null;

                ltRec2.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            List<LockingTest> allRecs = LockingTest.All().ToList();

            allRecs.ForEach(a => a.Delete());
        }


        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestTransactionRollback()
        {

            MyClassInitialize(this.TestContext);

            try
            {
                using (SharedDbConnectionScope sharedConnectionScope = new SharedDbConnectionScope())
                {

                    LockingTest rec = LockingTest.Find(lt => lt.FirstName == "Bruce").ToList()[0];

                    rec.FirstName = "BruceX";

                    rec.Save();

                    rec.FirstName = "BruceY";

                    // This should get blocked !
                    rec.Save();


                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Optimistic"))
                {
                    // Eat the exception
                }
                else
                {
                    throw ex;
                }

            }

            // Verify that the record still reads "Bruce", meaning the entire trans was rolled back
            LockingTest rec2 = LockingTest.Find(lt => lt.LastName == "Wolf").ToList()[0];

            Assert.AreEqual(rec2.FirstName, "Bruce");

        }

        [TestMethod]
        public void TestTransactionWorks()
        {

            MyClassInitialize(this.TestContext);

            try
            {
                using (SharedDbConnectionScope sharedConnectionScope = new SharedDbConnectionScope())
                {
                    LockingTest rec = LockingTest.Find(lt => lt.FirstName == "Bruce").ToList()[0];

                    rec.FirstName = "BruceX";

                    rec.Save();

                    // Re-pull from the db, so we get the new timestamp
                    rec = LockingTest.Find(lt => lt.FirstName == "BruceX").ToList()[0];

                    rec.FirstName = "BruceY";

                    // This should NOT get blocked !
                    rec.Save();

                    sharedConnectionScope.Commit();

                }
            }
            catch (Exception ex)
            {
                // Eat the exception
                //throw ex;
            }

            // Verify that the record still reads "Bruce", meaning the entire trans was rolled back
            LockingTest rec2 = LockingTest.Find(lt => lt.LastName == "Wolf").ToList()[0];

            Assert.AreEqual(rec2.FirstName, "BruceY");

        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Optimistic concurrency logic has blocked this update.")]
        public void TestOptimisticConcurrency()
        {
            MyClassInitialize(this.TestContext);

            try
            {
                LockingTest rec = LockingTest.Find(lt => lt.FirstName == "Bruce").ToList()[0];

                rec.FirstName = "BruceX";

                rec.Save();

                rec.FirstName = "BruceY";

                // This should get blocked !
                rec.Save();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [TestMethod]
        public void TestSingleUpdateInTrans()
        {

            MyClassInitialize(this.TestContext);

             LockingTest rec = LockingTest.Find(lt => lt.FirstName == "Bruce").ToList()[0];

                using (SharedDbConnectionScope sharedConnectionScope = new SharedDbConnectionScope())
                {
          
                    rec.FirstName = "BruceX";

                    rec.Save();

                    sharedConnectionScope.Commit();

                }
            

            // Verify that the record still reads "Bruce", meaning the entire trans was rolled back
            LockingTest rec2 = LockingTest.Find(lt => lt.LastName == "Wolf").ToList()[0];

            Assert.AreEqual(rec2.FirstName, "BruceX");

        }

        [TestMethod]
        public void TestSSTransactionRollbackUpdate()
        {

            MyClassInitialize(this.TestContext);

            try
            {
                using (SharedDbConnectionScope sharedConnectionScope = new SharedDbConnectionScope())
                {

                    LockingTest rec = LockingTest.Find(lt => lt.FirstName == "Bruce").ToList()[0];

                    rec.FirstName = "BruceX";

                    rec.Save();

                    rec.FirstName = "BruceY";

                    // This should get blocked !
                    rec.Save();

                    sharedConnectionScope.Commit();

                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Optimistic"))
                {
                    // Eat the exception
                }
                else
                {
                    throw ex;
                }

            }

            // Verify that the record still reads "Bruce", meaning the entire trans was rolled back
            LockingTest rec2 = LockingTest.Find(lt => lt.LastName == "Wolf").ToList()[0];

            Assert.AreEqual(rec2.FirstName, "Bruce");

        }

        [TestMethod]
        public void TestSSTransactionRollbackInsert()
        {

            MyClassInitialize(this.TestContext);

            int theCountBefore = 0;

            try
            {
                using (SharedDbConnectionScope sharedConnectionScope = new SharedDbConnectionScope())
                {
                    List<LockingTest> allRecs = LockingTest.All().ToList();

                    theCountBefore = allRecs.Count;

                    LockingTest newRec = new LockingTest();
                    newRec.FirstName = "aaa";
                    newRec.LastName = "bbb";
                    newRec.Age = 84;

                    newRec.Save();

                    throw new ApplicationException("Intentional Exception");

                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Intentional Exception"))
                {
                    // Eat the exception
                }
                else
                {
                    throw ex;
                }

            }

            List<LockingTest> allRecs2 = LockingTest.All().ToList();

            int theCountAfter = allRecs2.Count;

            // Verify that the record still reads "Bruce", meaning the entire trans was rolled back
            LockingTest rec2 = LockingTest.Find(lt => lt.LastName == "Wolf").ToList()[0];

            Assert.AreEqual(theCountBefore, theCountAfter);

        }

        [TestMethod]
        public void TestSSTransactionRollbackDelete()
        {

            MyClassInitialize(this.TestContext);

            int theCountBefore = 0;

            try
            {
                using (SharedDbConnectionScope sharedConnectionScope = new SharedDbConnectionScope())
                {
                    List<LockingTest> allRecs = LockingTest.All().ToList();

                    allRecs.ForEach(r => r.Delete());

                    throw new ApplicationException("Intentional Exception");

                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Intentional Exception"))
                {
                    // Eat the exception
                }
                else
                {
                    throw ex;
                }

            }

            List<LockingTest> allRecs2 = LockingTest.All().ToList();

            int theCountAfter = allRecs2.Count;

            // Verify that the record still reads "Bruce", meaning the entire trans was rolled back
            LockingTest rec2 = LockingTest.Find(lt => lt.LastName == "Wolf").ToList()[0];

            Assert.IsTrue(theCountAfter > 0);

        }

        [TestMethod]
        public void TestInsertThenTransDeleteRollback()
        {

            MyClassInitialize(this.TestContext);

            LockingTest newRec = new LockingTest();
            newRec.FirstName = "aaa";
            newRec.LastName = "bbb";
            newRec.Age = 84;

            newRec.Save();

            using (SharedDbConnectionScope sharedConnectionScope = new SharedDbConnectionScope())
            {

                // Delete all records
                LockingTest.All().ToList().ForEach(r => r.Delete());

                // DON'T Commit! Delete should roll back
                //sharedConnectionScope.Commit();

            }

            newRec = new LockingTest();
            newRec.FirstName = "aaa";
            newRec.LastName = "bbb";
            newRec.Age = 84;

            newRec.Save();

            using (SharedDbConnectionScope sharedConnectionScope = new SharedDbConnectionScope())
            {

                // Delete all records
                LockingTest.All().ToList().ForEach(r => r.Delete());

                // DON'T Commit! Delete should roll back
                //sharedConnectionScope.Commit();

            }


            List<LockingTest> allRecs2 = LockingTest.All().ToList();

            int theCountAfter = allRecs2.Count;

            Assert.AreEqual(theCountAfter, 4);

        }
    }
}
