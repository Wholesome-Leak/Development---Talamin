using robotManager.Helpful;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WholesomeDungeons.Bot;
using WholesomeDungeons.Helper;

namespace WholesomeDungeons.GUI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            ((INotifyCollectionChanged)MembersList.Items).CollectionChanged += ListView_CollectionChanged;
            Load();
        }
       
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
        void Load() 
        {
            try
            {
                TankRoleCheck.IsChecked = WholesomeDungeonsSettings.CurrentSetting?.SetAsTank ?? false;
                HealRoleChecl.IsChecked = WholesomeDungeonsSettings.CurrentSetting?.SetAsHeal ?? false;
                RDPSRoleCheck.IsChecked = WholesomeDungeonsSettings.CurrentSetting?.SetAsRDPS ?? false;
                MDPSRoleCheck.IsChecked = WholesomeDungeonsSettings.CurrentSetting?.SetAsMDPS ?? false;
                LootModeBosses.IsChecked = WholesomeDungeonsSettings.CurrentSetting?.SetLootBoss ?? false;
                LootModeTrash.IsChecked = WholesomeDungeonsSettings.CurrentSetting?.SetLootAll ?? false;
                FollowDistanceHeal.Text = WholesomeDungeonsSettings.CurrentSetting.FollowRangeHeal.ToString();
                FollowDistanceMDPS.Text = WholesomeDungeonsSettings.CurrentSetting.FollowRangeMDPS.ToString();
                FollowDistanceRDPS.Text = WholesomeDungeonsSettings.CurrentSetting.FollowRangeRDPS.ToString();
                //BuyFood.IsChecked = WholesomeDungeonsSettings.CurrentSetting?.AutobuyFood ?? false;
                //BuyDrink.IsChecked = WholesomeDungeonsSettings.CurrentSetting?.AutoBuyWater ?? false;
                //BuyPoison.IsChecked = WholesomeDungeonsSettings.CurrentSetting?.AllowAutobuyPoison ?? false;
                //BuyAmmo.IsChecked = WholesomeDungeonsSettings.CurrentSetting?.AutobuyAmmunition ?? false;
                //Sell.IsChecked = WholesomeDungeonsSettings.CurrentSetting?.AllowAutoSell ?? false;
                //Repair.IsChecked = WholesomeDungeonsSettings.CurrentSetting?.AutoRepair ?? false;
                //Train.IsChecked = WholesomeDungeonsSettings.CurrentSetting?.AutoTrain ?? false;
                //BuyAmmoAmount.Text = WholesomeDungeonsSettings.CurrentSetting.AutobuyAmmunitionAmount.ToString();
                SmoothMove.IsChecked = WholesomeDungeonsSettings.CurrentSetting?.Smoothmove ?? false;


                foreach (string item in WholesomeDungeonsSettings.CurrentSetting.GroupMembers)
                {
                    MembersList.Items.Add(item);
                }
                if(!string.IsNullOrWhiteSpace(WholesomeDungeonsSettings.CurrentSetting.TankName))
                {
                    MembersList.Items.Add(WholesomeDungeonsSettings.CurrentSetting.TankName);
                }
                if ((bool)TankRoleCheck.IsChecked == true)
                {
                    HealRoleChecl.Visibility = Visibility.Collapsed;
                    RDPSRoleCheck.Visibility = Visibility.Collapsed;
                    MDPSRoleCheck.Visibility = Visibility.Collapsed;
                }
                if ((bool)TankRoleCheck.IsChecked == false)
                {
                    HealRoleChecl.Visibility = Visibility.Visible;
                    RDPSRoleCheck.Visibility = Visibility.Visible;
                    MDPSRoleCheck.Visibility = Visibility.Visible;
                }
                if ((bool)HealRoleChecl.IsChecked == true)
                {
                    TankRoleCheck.Visibility = Visibility.Collapsed;
                    RDPSRoleCheck.Visibility = Visibility.Collapsed;
                    MDPSRoleCheck.Visibility = Visibility.Collapsed;
                }
                if ((bool)HealRoleChecl.IsChecked == false)
                {
                    TankRoleCheck.Visibility = Visibility.Visible;
                    RDPSRoleCheck.Visibility = Visibility.Visible;
                    MDPSRoleCheck.Visibility = Visibility.Visible;
                }
                if ((bool)RDPSRoleCheck.IsChecked == true)
                {
                    TankRoleCheck.Visibility = Visibility.Collapsed;
                    HealRoleChecl.Visibility = Visibility.Collapsed;
                    MDPSRoleCheck.Visibility = Visibility.Collapsed;
                }
                if ((bool)RDPSRoleCheck.IsChecked == false)
                {
                    TankRoleCheck.Visibility = Visibility.Visible;
                    HealRoleChecl.Visibility = Visibility.Visible;
                    MDPSRoleCheck.Visibility = Visibility.Visible;
                }
                if ((bool)MDPSRoleCheck.IsChecked == true)
                {
                    TankRoleCheck.Visibility = Visibility.Collapsed;
                    HealRoleChecl.Visibility = Visibility.Collapsed;
                    RDPSRoleCheck.Visibility = Visibility.Collapsed;
                }
                if ((bool)MDPSRoleCheck.IsChecked == false)
                {
                    TankRoleCheck.Visibility = Visibility.Visible;
                    HealRoleChecl.Visibility = Visibility.Visible;
                    RDPSRoleCheck.Visibility = Visibility.Visible;
                }
                ServerClient.IsChecked = WholesomeDungeonsSettings.CurrentSetting?.ServerClient ?? false;

            }
            catch (Exception e)
            {
                Logger.LogError("Load(): " + e);
            }
        }


 
        private void LootModeBosses_Checked(object sender, RoutedEventArgs e)
        {
            WholesomeDungeonsSettings.CurrentSetting.SetLootBoss = (bool)LootModeBosses.IsChecked;
        }

        private void LootModeTrash_Checked(object sender, RoutedEventArgs e)
        {
            WholesomeDungeonsSettings.CurrentSetting.SetLootAll = (bool)LootModeTrash.IsChecked;
        }

        private void ActivateDrawing_Checked()
        {

        }

        private void TankRoleCheck_Checked(object sender, RoutedEventArgs e)
        {
            if((bool)TankRoleCheck.IsChecked == true)
            {
                HealRoleChecl.Visibility = Visibility.Collapsed;
                RDPSRoleCheck.Visibility = Visibility.Collapsed;
                MDPSRoleCheck.Visibility = Visibility.Collapsed;
                HealRoleChecl.IsChecked = false;
                RDPSRoleCheck.IsChecked = false;
                MDPSRoleCheck.IsChecked = false;
            }
            if ((bool)TankRoleCheck.IsChecked == false)
            {
                HealRoleChecl.Visibility = Visibility.Visible;
                RDPSRoleCheck.Visibility = Visibility.Visible;
                MDPSRoleCheck.Visibility = Visibility.Visible;
            }
            WholesomeDungeonsSettings.CurrentSetting.SetAsTank = (bool)TankRoleCheck.IsChecked;
            Helpers.LUASetLFGRole();
        }

        private void HealRoleChecl_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)HealRoleChecl.IsChecked == true)
            {
                TankRoleCheck.Visibility = Visibility.Collapsed;
                RDPSRoleCheck.Visibility = Visibility.Collapsed;
                MDPSRoleCheck.Visibility = Visibility.Collapsed;
                TankRoleCheck.IsChecked = false;
                RDPSRoleCheck.IsChecked = false;
                MDPSRoleCheck.IsChecked = false;
            }
            if ((bool)HealRoleChecl.IsChecked == false)
            {
                TankRoleCheck.Visibility = Visibility.Visible;
                RDPSRoleCheck.Visibility = Visibility.Visible;
                MDPSRoleCheck.Visibility = Visibility.Visible;
            }
            WholesomeDungeonsSettings.CurrentSetting.SetAsHeal = (bool)HealRoleChecl.IsChecked;
            Helpers.LUASetLFGRole();
        }

        private void RDPSRoleCheck_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)RDPSRoleCheck.IsChecked == true)
            {
                TankRoleCheck.Visibility = Visibility.Collapsed;
                HealRoleChecl.Visibility = Visibility.Collapsed;
                MDPSRoleCheck.Visibility = Visibility.Collapsed;
                TankRoleCheck.IsChecked = false;
                HealRoleChecl.IsChecked = false;
                MDPSRoleCheck.IsChecked = false;
            }
            if ((bool)RDPSRoleCheck.IsChecked == false)
            {
                TankRoleCheck.Visibility = Visibility.Visible;
                HealRoleChecl.Visibility = Visibility.Visible;
                MDPSRoleCheck.Visibility = Visibility.Visible;
            }
                WholesomeDungeonsSettings.CurrentSetting.SetAsRDPS = (bool)RDPSRoleCheck.IsChecked;
                Helpers.LUASetLFGRole();
        }

        private void MDPSRoleCheck_Checked(object sender, RoutedEventArgs e) //MDPS
        {
            if ((bool)MDPSRoleCheck.IsChecked == true)
            {
                TankRoleCheck.Visibility = Visibility.Collapsed;
                HealRoleChecl.Visibility = Visibility.Collapsed;
                RDPSRoleCheck.Visibility = Visibility.Collapsed;
                TankRoleCheck.IsChecked = false;
                HealRoleChecl.IsChecked = false;
                RDPSRoleCheck.IsChecked = false;
            }
            if ((bool)MDPSRoleCheck.IsChecked == false)
            {
                TankRoleCheck.Visibility = Visibility.Visible;
                HealRoleChecl.Visibility = Visibility.Visible;
                RDPSRoleCheck.Visibility = Visibility.Visible;
            }
            WholesomeDungeonsSettings.CurrentSetting.SetAsMDPS = (bool)MDPSRoleCheck.IsChecked;
            Helpers.LUASetLFGRole();
        }

        private void ListView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Remove)
            {
                UpdateListViewColumns(MembersList);
            }
        }

        private void UpdateListViewColumns(ListView listView)
        {
            GridView gridView = listView.View as GridView;

            if (gridView != null)
                foreach (GridViewColumn column in gridView.Columns)
                {
                    column.Width = column.ActualWidth;
                    column.Width = double.NaN;
                }
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!MembersList.Items.Contains(NewMembersAdd.Text))
            MembersList.Items.Add(NewMembersAdd.Text);
            if(!WholesomeDungeonsSettings.CurrentSetting.GroupMembers.Contains(NewMembersAdd.Text))
            WholesomeDungeonsSettings.CurrentSetting.GroupMembers.Add(NewMembersAdd.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MembersList.Items.Clear();
            WholesomeDungeonsSettings.CurrentSetting.GroupMembers.Clear();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!MembersList.Items.Contains(NewTankAdd.Text))
                MembersList.Items.Add(NewTankAdd.Text);
            if (WholesomeDungeonsSettings.CurrentSetting.TankName != NewTankAdd.Text)
                WholesomeDungeonsSettings.CurrentSetting.TankName = NewTankAdd.Text;
        }

        private void NewMembersAdd_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void NewTankAdd_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SAVE_Click(object sender, RoutedEventArgs e)
        {
            WholesomeDungeonsSettings.CurrentSetting.Save();
        }

        private void RemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            if(MembersList.SelectedItems.Count > 0)
            {
                MembersList.Items.RemoveAt(MembersList.Items.IndexOf(MembersList.SelectedItem));
                WholesomeDungeonsSettings.CurrentSetting.GroupMembers.Clear();
                foreach (string item in MembersList.Items)
                {
                    WholesomeDungeonsSettings.CurrentSetting.GroupMembers.Add(item);
                }
                //WholesomeDungeonsSettings.CurrentSetting.GroupMembers = MembersList.Items;
            }
        }


        private void FollowDistanceHeal_TextChanged(object sender, TextChangedEventArgs e) //Followrange HEAL
        {
            int range = int.TryParse(FollowDistanceHeal.Text, out range) ? range : WholesomeDungeonsSettings.CurrentSetting.FollowRangeHeal;
            WholesomeDungeonsSettings.CurrentSetting.FollowRangeHeal = range; //  Convert.ToInt32(FollowDistanceHeal.Text);
        }

        private void ChooseRandomDungeon_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ChooseSpecificDungeon_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) //Dropdown specific Dungeon
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e) //Follow Distance RDPS
        {
            int range = int.TryParse(FollowDistanceRDPS.Text, out range) ? range : WholesomeDungeonsSettings.CurrentSetting.FollowRangeRDPS;
            WholesomeDungeonsSettings.CurrentSetting.FollowRangeRDPS = range;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) //Follow  Distance MDPS
        {
            int range = int.TryParse(FollowDistanceMDPS.Text, out range) ? range : WholesomeDungeonsSettings.CurrentSetting.FollowRangeMDPS;
            WholesomeDungeonsSettings.CurrentSetting.FollowRangeMDPS = range;
        }

        private void ServerClient_Checked(object sender, RoutedEventArgs e)
        {
            WholesomeDungeonsSettings.CurrentSetting.ServerClient = (bool)ServerClient.IsChecked;
        }


        ////Sell,buy,repair
        //private void BuyFood_Checked(object sender, RoutedEventArgs e)
        //{
        //    WholesomeDungeonsSettings.CurrentSetting.AutobuyFood = (bool)BuyFood.IsChecked;
        //}

        //private void BuyDrink_Checked(object sender, RoutedEventArgs e)
        //{
        //    WholesomeDungeonsSettings.CurrentSetting.AutoBuyWater = (bool)BuyDrink.IsChecked;
        //}

        //private void BuyPoison_Checked(object sender, RoutedEventArgs e)
        //{
        //    WholesomeDungeonsSettings.CurrentSetting.AllowAutobuyPoison = (bool)BuyPoison.IsChecked;
        //}

        //private void BuyAmmo_Checked(object sender, RoutedEventArgs e)
        //{
        //    WholesomeDungeonsSettings.CurrentSetting.AutobuyAmmunition = (bool)BuyAmmo.IsChecked;
        //}

        //private void Sell_Checked(object sender, RoutedEventArgs e)
        //{
        //    WholesomeDungeonsSettings.CurrentSetting.AllowAutoSell = (bool)Sell.IsChecked;
        //}

        //private void Repair_Checked(object sender, RoutedEventArgs e)
        //{
        //    WholesomeDungeonsSettings.CurrentSetting.AutoRepair = (bool)Repair.IsChecked;
        //}

        //private void Train_Checked(object sender, RoutedEventArgs e)
        //{
        //    WholesomeDungeonsSettings.CurrentSetting.AutoTrain = (bool)Train.IsChecked;
        //}

        //private void BuyAmmoAmount_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    int amount = int.TryParse(BuyAmmoAmount.Text, out amount) ? amount : WholesomeDungeonsSettings.CurrentSetting.AutobuyAmmunitionAmount;
        //    WholesomeDungeonsSettings.CurrentSetting.AutobuyAmmunitionAmount = amount;
        //}

        private void SmoothMove_Checked(object sender, RoutedEventArgs e)
        {
            WholesomeDungeonsSettings.CurrentSetting.Smoothmove = (bool)SmoothMove.IsChecked;
        }
    }
}
