using System;

using RLToolkit;
using RLToolkit.Extensions;
using NUnit.Framework;

namespace RLToolkit.UnitTests.Extensions
{
    [TestFixture]
    public class ToggleButtonExclusiveSelectionGroupTest : TestHarness, ITestBase
    {
        #region Local Variables

        #endregion

        #region Interface Override
        public string ModuleName()
        {

            return "ToggleButtonExclusiveSelectionGroup";
        }

        public override void SetFolderPaths()
        {
            localFolder = AppDomain.CurrentDomain.BaseDirectory;
            SetPaths (localFolder, ModuleName());
        }

        public override void DataPrepare()
        {

        }
        #endregion

        #region Tests-AddRemove
        [Test]
        public void TBESG_AddRemove_Append_Normal()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();

            bool result = grp.Append(new Gtk.ToggleButton());

            Assert.AreEqual(true, result, "ToggleButton Should be able to be added");
            Assert.AreEqual(1, grp.GetCountButton(), "There should be 1 button in the group");
        }

        [Test]
        public void TBESG_AddRemove_Append_Duplicate()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();

            bool result = grp.Append(toAdd);
            Assert.AreEqual(true, result, "ToggleButton Should be able to be added the first time");

            result = grp.Append(toAdd);
            Assert.AreEqual(false, result, "ToggleButton Should not be able to be added the second time");

            Assert.AreEqual(1, grp.GetCountButton(), "There should be 1 button in the group");
        }

        [Test]
        public void TBESG_AddRemove_Append_Multiple()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd1 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd2 = new Gtk.ToggleButton();

            bool result = grp.Append(toAdd1);
            Assert.AreEqual(true, result, "ToggleButton1 Should be able to be added");

            result = grp.Append(toAdd2);
            Assert.AreEqual(true, result, "ToggleButton2 Should not be able to be added");

            Assert.AreEqual(2, grp.GetCountButton(), "There should be 2 buttons in the group");
        }
        
        [Test]
        public void TBESG_AddRemove_Remove_Normal()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();

            bool result = grp.Append(toAdd);
            Assert.AreEqual(true, result, "ToggleButton1 Should be able to be added");

            result = grp.Remove(toAdd);

            Assert.AreEqual(true, result, "ToggleButton Should be removed properly");
            Assert.AreEqual(0, grp.GetCountButton(), "There should be no button in the group");
        }
                
        [Test]
        public void TBESG_AddRemove_Remove_Number()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();

            bool result = grp.Append(toAdd);
            Assert.AreEqual(true, result, "ToggleButton1 Should be able to be added");

            result = grp.Remove(0);

            Assert.AreEqual(true, result, "ToggleButton Should be removed properly");
            Assert.AreEqual(0, grp.GetCountButton(), "There should be no button in the group");
        }
        
        [Test]
        public void TBESG_AddRemove_Remove_NotExistant()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd2 = new Gtk.ToggleButton();

            bool result = grp.Append(toAdd);
            Assert.AreEqual(true, result, "ToggleButton1 Should be able to be added");

            result = grp.Remove(toAdd2);

            Assert.AreEqual(false, result, "Invalid button to remove, shouldn't work");
            Assert.AreEqual(1, grp.GetCountButton(), "There should be 1 button in the group");
        }

        [Test]
        public void TBESG_AddRemove_Remove_NotExistant_Number()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();

            bool result = grp.Append(toAdd);
            Assert.AreEqual(true, result, "ToggleButton1 Should be able to be added");

            result = grp.Remove(4);

            Assert.AreEqual(false, result, "Invalid button to remove, shouldn't work");
            Assert.AreEqual(1, grp.GetCountButton(), "There should be 1 button in the group");
        }

        [Test]
        public void TBESG_AddRemove_Remove_Invalid_Low()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            bool result = grp.Remove(-4);

            Assert.AreEqual(false, result, "Invalid button to remove, shouldn't work");
            Assert.AreEqual(0, grp.GetCountButton(), "There should be no button in the group");
        }

        [Test]
        public void TBESG_AddRemove_Remove_Invalid_High()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            bool result = grp.Remove(4);

            Assert.AreEqual(false, result, "Invalid button to remove, shouldn't work");
            Assert.AreEqual(0, grp.GetCountButton(), "There should be no button in the group");
        }

        [Test]
        public void TBESG_AddRemove_Remove_BigList()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd2 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd3 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd4 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd5 = new Gtk.ToggleButton();

            grp.Append(toAdd);
            grp.Append(toAdd2);
            grp.Append(toAdd3);
            grp.Append(toAdd4);
            grp.Append(toAdd5);

            Assert.AreEqual(5, grp.GetCountButton(), "There should be 5 buttons in the group");

            // remove the 3rd one
            bool result = grp.Remove(toAdd3);
            Assert.AreEqual(true, result, "3rd button removal should work");
            Assert.AreEqual(4, grp.GetCountButton(), "There should be 4 buttons in the group");

            // make sure the 3 is no longer there
            int pos = grp.FindInGroup(toAdd3);
            Assert.AreEqual(-1, pos, "The button should not be found anymore");
        }

        [Test]
        public void TBESG_AddRemove_RemoveAll()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd2 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd3 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd4 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd5 = new Gtk.ToggleButton();

            grp.Append(toAdd);
            grp.Append(toAdd2);
            grp.Append(toAdd3);
            grp.Append(toAdd4);
            grp.Append(toAdd5);

            Assert.AreEqual(5, grp.GetCountButton(), "There should be 5 buttons in the group");
            grp.Select(toAdd3);
            Assert.AreEqual(true, toAdd3.Active, "Control 3 should be selected.");

            grp.RemoveAll();
            Assert.AreEqual(0, grp.GetCountButton(), "There should be no button in the group");
            Assert.AreEqual(false, toAdd3.Active, "Control 3 should no longer be selected.");
        }


        #endregion

        #region Tests-Select
        [Test]
        public void TBESG_Select_Normal()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd2 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd3 = new Gtk.ToggleButton();

            grp.Append(toAdd);
            grp.Append(toAdd2);
            grp.Append(toAdd3);
            Assert.AreEqual(3, grp.GetCountButton(), "There should be 3 buttons in the group");

            bool result = grp.Select(0);
            Assert.AreEqual(true, result, "Selection of index 0 should work after first test");
            Assert.AreEqual(true, toAdd.Active, "control 1 should be active after first test");
            Assert.AreEqual(false, toAdd3.Active, "control 3 should not be active after first test");

            result = grp.Select(2);
            Assert.AreEqual(true, result, "Selection of index 2 should work after second test");
            Assert.AreEqual(false, toAdd.Active, "control 1 should not be active after second test");
            Assert.AreEqual(true, toAdd3.Active, "control 3 should be active after second test");
        }

        [Test]
        public void TBESG_Select_WithControl()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd2 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd3 = new Gtk.ToggleButton();

            grp.Append(toAdd);
            grp.Append(toAdd2);
            grp.Append(toAdd3);
            Assert.AreEqual(3, grp.GetCountButton(), "There should be 3 buttons in the group");

            bool result = grp.Select(toAdd);
            Assert.AreEqual(true, result, "Selection of index 0 should work after first test");
            Assert.AreEqual(true, toAdd.Active, "control 1 should be active after first test");
            Assert.AreEqual(false, toAdd3.Active, "control 3 should not be active after first test");

            result = grp.Select(toAdd3);
            Assert.AreEqual(true, result, "Selection of index 2 should work after second test");
            Assert.AreEqual(false, toAdd.Active, "control 1 should not be active after second test");
            Assert.AreEqual(true, toAdd3.Active, "control 3 should be active after second test");
        }

        [Test]
        public void TBESG_Select_SameIndex()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd2 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd3 = new Gtk.ToggleButton();

            grp.Append(toAdd);
            grp.Append(toAdd2);
            grp.Append(toAdd3);
            Assert.AreEqual(3, grp.GetCountButton(), "There should be 3 buttons in the group");

            bool result = grp.Select(1);
            Assert.AreEqual(true, result, "Selection of index 1 should work");

            result = grp.Select(1);
            Assert.AreEqual(false, result, "Selection of index 1 should not work twice since it's already selected");
        }

        [Test]
        public void TBESG_Select_TooHigh()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd2 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd3 = new Gtk.ToggleButton();

            grp.Append(toAdd);
            grp.Append(toAdd2);
            grp.Append(toAdd3);
            Assert.AreEqual(3, grp.GetCountButton(), "There should be 3 buttons in the group");

            bool result = grp.Select(6);
            Assert.AreEqual(false, result, "Selection of index 6 is too high");
        }

        [Test]
        public void TBESG_Select_TooLow()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd2 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd3 = new Gtk.ToggleButton();

            grp.Append(toAdd);
            grp.Append(toAdd2);
            grp.Append(toAdd3);
            Assert.AreEqual(3, grp.GetCountButton(), "There should be 3 buttons in the group");

            bool result = grp.Select(-5);
            Assert.AreEqual(false, result, "Selection of index -5 is too low");
        }

        [Test]
        public void TBESG_Select_LikeUnselected()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd2 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd3 = new Gtk.ToggleButton();

            grp.Append(toAdd);
            grp.Append(toAdd2);
            grp.Append(toAdd3);
            Assert.AreEqual(3, grp.GetCountButton(), "There should be 3 buttons in the group");

            bool result = grp.Select(-1);
            Assert.AreEqual(false, result, "Selection of index -1 should not work");
        }

        [Test]
        public void TBESG_Select_ControlNotExist()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd2 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd3 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAddX = new Gtk.ToggleButton();

            grp.Append(toAdd);
            grp.Append(toAdd2);
            grp.Append(toAdd3);
            Assert.AreEqual(3, grp.GetCountButton(), "There should be 3 buttons in the group");

            bool result = grp.Select(toAddX);
            Assert.AreEqual(false, result, "Selection of control X should not work after first test");
            Assert.AreEqual(false, toAddX.Active, "control 3 should not be active after first test");

        }
        #endregion

        #region Tests-Unselect
        [Test]
        public void TBESG_Unselect_Unselected()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd2 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd3 = new Gtk.ToggleButton();

            grp.Append(toAdd);
            grp.Append(toAdd2);
            grp.Append(toAdd3);
            Assert.AreEqual(3, grp.GetCountButton(), "There should be 3 buttons in the group");

            bool result = grp.Unselect();
            Assert.AreEqual(false, result, "Nothing selected, should not be able to unselect");
        }

        [Test]
        public void TBESG_Unselect_Normal()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd2 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd3 = new Gtk.ToggleButton();

            grp.Append(toAdd);
            grp.Append(toAdd2);
            grp.Append(toAdd3);
            Assert.AreEqual(3, grp.GetCountButton(), "There should be 3 buttons in the group");

            bool result = grp.Select(toAdd2);
            Assert.AreEqual(true, result, "Selection of index 1 should work");
            Assert.AreEqual(true, toAdd2.Active, "control 2 should be active");

            result = grp.Unselect();
            Assert.AreEqual(true, result, "Unselection should be sucessful");
            Assert.AreEqual(false, toAdd2.Active, "control 2 should no longer be active");
        }
        #endregion

        #region Tests-FindInGroup
        [Test]
        public void TBESG_FindInGroup_Normal()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd2 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd3 = new Gtk.ToggleButton();

            grp.Append(toAdd);
            grp.Append(toAdd2);
            grp.Append(toAdd3);
            Assert.AreEqual(3, grp.GetCountButton(), "There should be 3 buttons in the group");

            int result = grp.FindInGroup(toAdd2);
            Assert.AreEqual(1, result, "The FindInGroup method should return the right control index");
        }

        [Test]
        public void TBESG_FindInGroup_NotExist()
        {
            ToggleButtonExclusiveSelectionGroup grp = new ToggleButtonExclusiveSelectionGroup();
            Gtk.ToggleButton toAdd = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd2 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAdd3 = new Gtk.ToggleButton();
            Gtk.ToggleButton toAddX = new Gtk.ToggleButton();

            grp.Append(toAdd);
            grp.Append(toAdd2);
            grp.Append(toAdd3);
            Assert.AreEqual(3, grp.GetCountButton(), "There should be 3 buttons in the group");

            int result = grp.FindInGroup(toAddX);
            Assert.AreEqual(-1, result, "The FindInGroup method shouldn't find the control requested, returning -1");
        }
        #endregion
    }
}

