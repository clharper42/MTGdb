using System;
using System.Collections.Generic;
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

namespace MTGdb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GridViewColumnHeader listViewSortColPrice = null;
        private GridViewColumnHeader listViewSortCol = null;
        private bool ascending = false;
        private bool ascendingprice = false;
        public MainWindow()
        {
            Program.Start();
            InitializeComponent();
            CardDisp.ItemsSource = Program.allcards;
            CardDispPrice.ItemsSource = Program.allcards;
            ////CardDispColorCMC.ItemsSource = Program.allcards;
            ////CardDispColor.ItemsSource = Program.allcards;

            //CollectionView view = (CollectionView)new CollectionViewSource { Source = Program.allcards }.View;
            //PropertyGroupDescription groupDescription = new PropertyGroupDescription("Coloridcmc");
            //view.GroupDescriptions.Add(groupDescription);
            //CardDispColorCMC.ItemsSource = view;

            //view = (CollectionView)new CollectionViewSource { Source = Program.allcards }.View;
            //groupDescription = new PropertyGroupDescription("Colorstring");
            //view.GroupDescriptions.Add(groupDescription);
            //CardDispColorCMC.ItemsSource = null; //IMPORTANT
            //CardDispColor.ItemsSource = view;

            //view = (CollectionView)new CollectionViewSource { Source = Program.cardsbytype }.View;
            //groupDescription = new PropertyGroupDescription("Coloridfilter");
            //view.GroupDescriptions.Add(groupDescription);
            //CardDispColorCMC.ItemsSource = null; //IMPORTANT
            //CardDispColorType.ItemsSource = view;

            //view = (CollectionView)new CollectionViewSource { Source = Program.cardsbysubtype }.View;
            //groupDescription = new PropertyGroupDescription("Coloridfilter");
            //view.GroupDescriptions.Add(groupDescription);
            //CardDispColorCMC.ItemsSource = null; //IMPORTANT
            //CardDispColorSubtype.ItemsSource = view;

            //view = (CollectionView)new CollectionViewSource { Source = Program.cardsbykeyword }.View;
            //groupDescription = new PropertyGroupDescription("Coloridfilter");
            //view.GroupDescriptions.Add(groupDescription);
            //CardDispColorCMC.ItemsSource = null; //IMPORTANT
            //CardDispColorKeyword.ItemsSource = view;




            //google collectionview filtering wtih groupdescription

            //Program.Display(CardDisp);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);
            string selection = lbi.Content.ToString()[0].ToString();

            CardDisp.Visibility = Visibility.Hidden;
            CardDispPrice.Visibility = Visibility.Hidden;
            CardDispColor.Visibility = Visibility.Hidden;
            CardDispColorType.Visibility = Visibility.Hidden;
            CardDispColorSubtype.Visibility = Visibility.Hidden;
            CardDispColorCMC.Visibility = Visibility.Hidden;
            CardDispColorKeyword.Visibility = Visibility.Hidden;

            CardImg.Source = null;
            CardImgF1.Source = null;
            CardImgF2.Source = null;

            CardDispColor.ItemsSource = null;
            CardDispColorType.ItemsSource = null;
            CardDispColorSubtype.ItemsSource = null;
            CardDispColorCMC.ItemsSource = null;
            CardDispColorKeyword.ItemsSource = null;

            if (selection.Equals("1"))
            {
                CardDisp.Visibility = Visibility.Visible;
            }
            else if(selection.Equals("2"))
            {
                CardDispColor.Visibility = Visibility.Visible;
                CollectionView view = (CollectionView)new CollectionViewSource { Source = Program.cardsbycolorandcmc }.View;
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Colorstring");
                view.GroupDescriptions.Add(groupDescription);
                CardDispColor.ItemsSource = view;
            }
            else if (selection.Equals("3"))
            {
                CardDispColorType.Visibility = Visibility.Visible;
                CollectionView view = (CollectionView)new CollectionViewSource { Source = Program.cardsbytype }.View;
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Coloridfilter");
                view.GroupDescriptions.Add(groupDescription);
                CardDispColorType.ItemsSource = view;
            }
            else if (selection.Equals("4"))
            {
                CardDispColorSubtype.Visibility = Visibility.Visible;
                CollectionView view = (CollectionView)new CollectionViewSource { Source = Program.cardsbysubtype }.View;
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Coloridfilter");
                view.GroupDescriptions.Add(groupDescription);
                CardDispColorSubtype.ItemsSource = view;
            }
            else if (selection.Equals("5"))
            {
                CardDispColorCMC.Visibility = Visibility.Visible;
                CollectionView view = (CollectionView)new CollectionViewSource { Source = Program.cardsbycolorandcmc }.View;
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Coloridcmc");
                view.GroupDescriptions.Add(groupDescription);
                CardDispColorCMC.ItemsSource = view;
            }
            else if (selection.Equals("6"))
            {
                CardDispColorKeyword.Visibility = Visibility.Visible;
                CollectionView view = (CollectionView)new CollectionViewSource { Source = Program.cardsbykeyword }.View;
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Coloridfilter");
                view.GroupDescriptions.Add(groupDescription);
                CardDispColorKeyword.ItemsSource = view;
            }
            else if (selection.Equals("7"))
            {
                CardDispPrice.Visibility = Visibility.Visible;
            }

        }

        private void CardDisp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0)
            {
                Card selc = e.AddedItems[0] as Card;
                if (!(selc.Image_uris is null))
                {
                    CardImgF1.Source = null;
                    CardImgF2.Source = null;
                    CardImg.Source = new BitmapImage(new Uri(selc.Image_uris["normal"]));
                }
                else
                {
                    CardImg.Source = null;
                    CardImgF1.Source = new BitmapImage(new Uri(selc.Card_faces[0].Image_uris["normal"]));
                    CardImgF2.Source = new BitmapImage(new Uri(selc.Card_faces[1].Image_uris["normal"]));
                }
            }
            //ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);
            //string cardsel = lbi.Content.ToString();
            //CardImg.Source = new BitmapImage(new Uri(Program.allcards[Convert.ToInt32(cardsel[cardsel.Length - 1].ToString())].Image_uris["normal"]));
        }

        private void CardDispColorFil_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Card selc = (e.AddedItems[0] as CardWithFilter).Thecard;
                if (!(selc.Image_uris is null))
                {
                    CardImgF1.Source = null;
                    CardImgF2.Source = null;
                    CardImg.Source = new BitmapImage(new Uri(selc.Image_uris["normal"]));
                }
                else
                {
                    CardImg.Source = null;
                    CardImgF1.Source = new BitmapImage(new Uri(selc.Card_faces[0].Image_uris["normal"]));
                    CardImgF2.Source = new BitmapImage(new Uri(selc.Card_faces[1].Image_uris["normal"]));
                }
            }
            //ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);
            //string cardsel = lbi.Content.ToString();
            //CardImg.Source = new BitmapImage(new Uri(Program.allcards[Convert.ToInt32(cardsel[cardsel.Length - 1].ToString())].Image_uris["normal"]));
        }

        private void CardDispPriceColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();

            if (listViewSortColPrice != null)
            {
                CardDispPrice.Items.SortDescriptions.Clear();
            }

            //System.ComponentModel.ListSortDirection newDir = System.ComponentModel.ListSortDirection.Ascending;
            //if (listViewSortCol == column)
            //{
            //    newDir = System.ComponentModel.ListSortDirection.Descending;
            //}

            System.ComponentModel.ListSortDirection newDir;
            if (!ascendingprice)
            {
                newDir = System.ComponentModel.ListSortDirection.Ascending;
                ascendingprice = true;
            }
            else
            {
                newDir = System.ComponentModel.ListSortDirection.Descending;
                ascendingprice = false;
            }

            listViewSortColPrice = column;
            CardDispPrice.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription(sortBy, newDir));
        }

        private void CardDispColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();

            if (listViewSortCol != null)
            {
                CardDisp.Items.SortDescriptions.Clear();
            }

            //System.ComponentModel.ListSortDirection newDir = System.ComponentModel.ListSortDirection.Ascending;
            //if (listViewSortCol == column)
            //{
            //    newDir = System.ComponentModel.ListSortDirection.Descending;
            //}

            System.ComponentModel.ListSortDirection newDir;
            if (!ascending)
            {
                newDir = System.ComponentModel.ListSortDirection.Ascending;
                ascending = true;
            }
            else
            {
                newDir = System.ComponentModel.ListSortDirection.Descending;
                ascending = false;
            }

            listViewSortCol = column;
            CardDisp.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription(sortBy, newDir));
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

    }
}
