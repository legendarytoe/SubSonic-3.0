using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SubSonic.DataProviders;
using SubsonicLockingUnitTests;

namespace SubsonicAddonSampleApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dgData.AutoGenerateColumns = false;

            RefreshGrid();
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            LockingTest.All().ToList().ForEach(r => r.Delete());
            RefreshGrid();
        }

        private void btnAddRecords_Click(object sender, EventArgs e)
        {
            LockingTest rec = new LockingTest();
            rec.FirstName = "Lancaster";
            rec.LastName = "Gordon";
            rec.Age = 50;

            rec.Save();

            rec = new LockingTest();
            rec.FirstName = "Wiley";
            rec.LastName = "Brown";
            rec.Age = 51;

            rec.Save();

            RefreshGrid();
        }

        private void btnRollback_Click(object sender, EventArgs e)
        {

            using (SharedDbConnectionScope sharedConnectionScope = new SharedDbConnectionScope())
            {
                string numRecs = LockingTest.All().ToList().Count.ToString();

                MessageBox.Show(string.Format("There are {0} records before the delete", numRecs));

                // Delete all records
                LockingTest.All().ToList().ForEach(r => r.Delete());

                numRecs = LockingTest.All().ToList().Count.ToString();

                MessageBox.Show(
                    string.Format(
                        "There are {0} records after the delete, but before the transaction is committed", numRecs));

                // DON'T Commit the transaction so we can see the rollback
                // trans.SSTransCommit();
            }

            string numRecs2 = LockingTest.All().ToList().Count.ToString();

            MessageBox.Show(string.Format("There are {0} records after the rollback", numRecs2));

            RefreshGrid();

        }

        private void RefreshGrid()
        {

            List<LockingTest> allRecs = LockingTest.All().ToList();

            dgData.DataSource = allRecs;
        }

        private void btnRefreshGrid_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }


    }
}
