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
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;
using System.Collections.ObjectModel;

namespace MTGdb
{
    public partial class MainWindow : Window
    {
        private GridViewColumnHeader listViewSortColPrice = null;
        private GridViewColumnHeader listViewSortCol = null;
        private bool ascending = false;
        private bool ascendingprice = false;
        private string colorsfil = "";
        private bool contains = false;
        private bool exactcardname = false;
        private bool exacttype = false;
        private bool exactsubtype = false;
        private bool exactkeyword = false;
        private IEnumerable theitemsource;
        private Card selectedcard;
        private ListView currentlistview;
        IEnumerable converttooc;
        //list stuff
        private List<int> cardidsincurlist = new List<int>(); //clear out on switch
        private CardList selectedlist;
        private bool createlist = false;
        private bool dispalypage = true;
        private string filedir = Directory.GetCurrentDirectory() + "\\Files";
        private List<LoadableFile> loadablefilepaths = new List<LoadableFile>();
        private string selectedloadablefile = "";
        public MainWindow()
        {
            Program.Start();
            if (Program.missfiledir)
            {
                MessageBox.Show("Mising 'Files' Directory");
                System.Windows.Application.Current.Shutdown();
            }
            else if (Program.missfiles)
            {
                MessageBox.Show("Mising Carddb.csv and/or TCGplayer.csv In 'Files' Directory");
                System.Windows.Application.Current.Shutdown();
            }

            InitializeComponent();
            converttooc = new ObservableCollection<Card>(Program.allcards);
            CardDisp.ItemsSource = converttooc;
            CardDispPrice.ItemsSource = converttooc;
            currentlistview = CardDisp;

            theitemsource = CardDisp.ItemsSource;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(theitemsource);
            view.Filter = MainDisplay_Filter;


            //list stuff
            ListSelc.ItemsSource = Program.cardlists;
            CardDispCreateList.ItemsSource = Program.allcards;
            LoadListSelc.ItemsSource = loadablefilepaths;

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);

            if(!(lbi is null))
            {
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

                RemoveCardBtn.Visibility = Visibility.Hidden;
                Removetxt.Visibility = Visibility.Hidden;
                RemoveBox.Visibility = Visibility.Hidden;
                RemoveBox.Text = "";

                CardDispColor.ItemsSource = null;
                CardDispColorType.ItemsSource = null;
                CardDispColorSubtype.ItemsSource = null;
                CardDispColorCMC.ItemsSource = null;
                CardDispColorKeyword.ItemsSource = null;

                currentlistview.UnselectAll();

                if (selection.Equals("1"))
                {
                    //Main Dispaly
                    CardDisp.Visibility = Visibility.Visible;
                    theitemsource = CardDisp.ItemsSource;
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(theitemsource);
                    view.Filter = MainDisplay_Filter;

                    RemoveCardBtn.Visibility = Visibility.Visible;
                    Removetxt.Visibility = Visibility.Visible;
                    RemoveBox.Visibility = Visibility.Visible;

                    currentlistview = CardDisp;
                }
                else if (selection.Equals("2"))
                {
                    //Cards By Color
                    CardDispColor.Visibility = Visibility.Visible;
                    CollectionView view = (CollectionView)new CollectionViewSource { Source = new ObservableCollection<Card>(Program.cardsbycolorandcmc) }.View;
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Colorstring");
                    view.GroupDescriptions.Add(groupDescription);
                    CardDispColor.ItemsSource = view;

                    theitemsource = CardDispColor.ItemsSource;
                    view.Filter = MainDisplay_Filter;

                    currentlistview = CardDispColor;

                }
                else if (selection.Equals("3"))
                {
                    //Cards By Type
                    CardDispColorType.Visibility = Visibility.Visible;
                    CollectionView view = (CollectionView)new CollectionViewSource { Source = Program.cardsbytype }.View;
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Coloridfilter");
                    view.GroupDescriptions.Add(groupDescription);
                    CardDispColorType.ItemsSource = view;

                    theitemsource = CardDispColorType.ItemsSource;
                    view.Filter = GroupDispaly_Filter;

                    currentlistview = CardDispColorType;
                }
                else if (selection.Equals("4"))
                {
                    //Cards By Subtype
                    CardDispColorSubtype.Visibility = Visibility.Visible;
                    CollectionView view = (CollectionView)new CollectionViewSource { Source = Program.cardsbysubtype }.View;
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Coloridfilter");
                    view.GroupDescriptions.Add(groupDescription);
                    CardDispColorSubtype.ItemsSource = view;

                    theitemsource = CardDispColorSubtype.ItemsSource;
                    view.Filter = GroupDispaly_Filter;

                    currentlistview = CardDispColorSubtype;
                }
                else if (selection.Equals("5"))
                {
                    //Cards By CMC
                    CardDispColorCMC.Visibility = Visibility.Visible;
                    CollectionView view = (CollectionView)new CollectionViewSource { Source = Program.cardsbycolorandcmc }.View;
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Coloridcmc");
                    view.GroupDescriptions.Add(groupDescription);
                    CardDispColorCMC.ItemsSource = view;

                    theitemsource = CardDispColorCMC.ItemsSource;
                    view.Filter = MainDisplay_Filter;

                    currentlistview = CardDispColorCMC;
                }
                else if (selection.Equals("6"))
                {
                    //Cards By Keyword
                    CardDispColorKeyword.Visibility = Visibility.Visible;
                    CollectionView view = (CollectionView)new CollectionViewSource { Source = Program.cardsbykeyword }.View;
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Coloridfilter");
                    view.GroupDescriptions.Add(groupDescription);
                    CardDispColorKeyword.ItemsSource = view;

                    theitemsource = CardDispColorKeyword.ItemsSource;
                    view.Filter = GroupDispaly_Filter;

                    currentlistview = CardDispColorKeyword;
                }
                else if (selection.Equals("7"))
                {
                    //Cards By Price
                    CardDispPrice.Visibility = Visibility.Visible;
                    theitemsource = CardDispPrice.ItemsSource;
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(theitemsource);
                    view.Filter = MainDisplay_Filter;

                    currentlistview = CardDispPrice;
                }
                else if (selection.Equals("8"))
                {
                    //Switch To List View
                    CardViewSelet.UnselectAll();

                    CardViewSelet.Visibility = Visibility.Hidden;
                    ListViewSelect.Visibility = Visibility.Visible;

                    selectedcard = null;

                    theitemsource = CardDispCreateList.ItemsSource;
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(theitemsource);
                    view.Filter = MainDisplay_Filter;

                    ListSelc.Visibility = Visibility.Visible;
                    ListCardDispCreate.Visibility = Visibility.Visible;
                    CardDispCreateList.Visibility = Visibility.Visible;
                    ListName.Visibility = Visibility.Visible;
                    ListNameBox.Visibility = Visibility.Visible;
                    ListDes.Visibility = Visibility.Visible;
                    ListDescBox.Visibility = Visibility.Visible;
                    ListCardID.Visibility = Visibility.Visible;
                    ListCardAmout.Visibility = Visibility.Visible;
                    CardDBIDBox.Visibility = Visibility.Visible;
                    CardAmountBox.Visibility = Visibility.Visible;
                    CreateSaveBtn.Visibility = Visibility.Visible;
                    AddCardToListBtn.Visibility = Visibility.Visible;
                    UpdateCardInListBtn.Visibility = Visibility.Visible;
                    RemoveCardListBtn.Visibility = Visibility.Visible;
                    DeleteListBtn.Visibility = Visibility.Visible;


                    CreateSaveBtn.Content = "Save";
                    ListCardDispCreate.Height = 219;
                    Thickness margin = ListCardDispCreate.Margin;
                    margin.Left = 10;
                    margin.Top = 50;
                    margin.Left = 10;
                    ListCardDispCreate.Margin = margin;
                    ListSelc.Visibility = Visibility.Visible;
                    dispalypage = true;
                    createlist = false;
                    ListSelc.Items.Refresh();
                    ListSelc.SelectedIndex = -1;

                }
            }
            

        }

        private void CardDisp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Card Selection Changed
            if (e.AddedItems.Count > 0)
            {
                Card selc = e.AddedItems[0] as Card;
                selectedcard = selc;
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
        }

        private void CardDispColorFil_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Card Selection Changed On Grouped View
            if (e.AddedItems.Count > 0)
            {
                Card selc = (e.AddedItems[0] as CardWithFilter).Thecard;
                selectedcard = selc;
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
        }

        private void CardDispPriceColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            //Sorting View
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();

            if (listViewSortColPrice != null)
            {
                CardDispPrice.Items.SortDescriptions.Clear();
            }


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
            //Sorting View
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();

            if (listViewSortCol != null)
            {
                CardDisp.Items.SortDescriptions.Clear();
            }

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
            //Drag Display Around
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void W_CheckedEvent(object sender, RoutedEventArgs e)
        {
            //Add Color To Filter
            if (!colorsfil.Contains("W"))
            {
                colorsfil = colorsfil + "W";
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void W_UncheckedEvent(object sender, RoutedEventArgs e)
        {
            if (colorsfil.Contains("W"))
            {
                List<char> templist = colorsfil.ToList();
                templist.Remove('W');
                colorsfil = string.Join("", templist);
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void U_CheckedEvent(object sender, RoutedEventArgs e)
        {
            //Add Color To Filter
            if (!colorsfil.Contains("U"))
            {
                colorsfil = colorsfil + "U";
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void U_UncheckedEvent(object sender, RoutedEventArgs e)
        {
            if (colorsfil.Contains("U"))
            {
                List<char> templist = colorsfil.ToList();
                templist.Remove('U');
                colorsfil = string.Join("", templist);
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void B_CheckedEvent(object sender, RoutedEventArgs e)
        {
            //Add Color To Filter
            if (!colorsfil.Contains("B"))
            {
                colorsfil = colorsfil + "B";
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void B_UncheckedEvent(object sender, RoutedEventArgs e)
        {
            if (colorsfil.Contains("B"))
            {
                List<char> templist = colorsfil.ToList();
                templist.Remove('B');
                colorsfil = string.Join("", templist);
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void R_CheckedEvent(object sender, RoutedEventArgs e)
        {
            //Add Color To Filter
            if (!colorsfil.Contains("R"))
            {
                colorsfil = colorsfil + "R";
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void R_UncheckedEvent(object sender, RoutedEventArgs e)
        {
            if (colorsfil.Contains("R"))
            {
                List<char> templist = colorsfil.ToList();
                templist.Remove('R');
                colorsfil = string.Join("", templist);
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void G_CheckedEvent(object sender, RoutedEventArgs e)
        {
            //Add Color To Filter
            if (!colorsfil.Contains("G"))
            {
                colorsfil = colorsfil + "G";
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void G_UncheckedEvent(object sender, RoutedEventArgs e)
        {
            if (colorsfil.Contains("G"))
            {
                List<char> templist = colorsfil.ToList();
                templist.Remove('G');
                colorsfil = string.Join("", templist);
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void C_CheckedEvent(object sender, RoutedEventArgs e)
        {
            //Add Color To Filter
            if (!colorsfil.Contains("C"))
            {
                colorsfil = colorsfil + "C";
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void C_UncheckedEvent(object sender, RoutedEventArgs e)
        {
            if (colorsfil.Contains("C"))
            {
                List<char> templist = colorsfil.ToList();
                templist.Remove('C');
                colorsfil = string.Join("", templist);
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }


        private void Contains_CheckedEvent(object sender, RoutedEventArgs e)
        {
            //Sets Filter To See If Card Contains Atleast One Of The Selected Colors
            contains = true;
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void Contains_UncheckedEvent(object sender, RoutedEventArgs e)
        {
            contains = false;
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void ExactCardName_CheckedEvent(object sender, RoutedEventArgs e)
        {
            exactcardname = true;
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void ExactCardName_UncheckedEvent(object sender, RoutedEventArgs e)
        {
            exactcardname = false;
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void ExactType_CheckedEvent(object sender, RoutedEventArgs e)
        {
            exacttype = true;
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void ExactType_UncheckedEvent(object sender, RoutedEventArgs e)
        {
            exacttype = false;
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void ExactSubtype_CheckedEvent(object sender, RoutedEventArgs e)
        {
            exactsubtype = true;
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void ExactSubtype_UncheckedEvent(object sender, RoutedEventArgs e)
        {
            exactsubtype = false;
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void ExactKeyword_CheckedEvent(object sender, RoutedEventArgs e)
        {
            exactkeyword = true;
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void ExactKeyword_UncheckedEvent(object sender, RoutedEventArgs e)
        {
            exactkeyword = false;
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }
        private void CardName_Textchanged(object sender, TextChangedEventArgs e)
        {
            //Filter By Card Name
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void Type_Textchanged(object sender, TextChangedEventArgs e)
        {
            //Filter By Card Type
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void Subtype_Textchanged(object sender, TextChangedEventArgs e)
        {
            //Filter By Card Subtype
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void Keyword_Textchanged(object sender, TextChangedEventArgs e)
        {
            //Filter By Card Keyword
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void CMC_Textchanged(object sender, TextChangedEventArgs e)
        {
            //Filter By Card CMC
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void Textbox_Textchanged(object sender, TextChangedEventArgs e)
        {
            //Filter By Card Textbox
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private bool CheckCard(Card card)
        {
            //Fitler
            bool pass = false;
            if (colorsfil.Length > 0)
            {
                if (!colorsfil.Contains("C") || contains)
                {
                    if (contains)
                    {
                        foreach (char color in colorsfil)
                        {
                            if (color.Equals('C') && card.Color_identity.Count == 0)
                            {
                                pass = true;
                                break;
                            }
                            else if (card.Color_identity.Contains(color))
                            {
                                pass = true;
                                break;
                            }
                        }

                        if (!pass)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (colorsfil.Length == card.Color_identity.Count)
                        {
                            foreach (char color in colorsfil)
                            {
                                if (!card.Color_identity.Contains(color))
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else if (colorsfil.Length > 1) //Card Can't be colorless and be a color
                {
                    return false;
                }

            }

            if (!String.IsNullOrEmpty(CardNameBox.Text))
            {
                if (exactcardname)
                {
                    if (!card.Name.ToLower().Equals(CardNameBox.Text.ToLower()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!Regex.IsMatch(card.Name.ToLower(), CardNameBox.Text.ToLower()))
                    {
                        return false;
                    }
                }
            }

            pass = false;
            if (!String.IsNullOrEmpty(TypeBox.Text))
            {
                if (exacttype)
                {
                    if (card.Card_faces is null)
                    {
                        if (card.Type_line.Contains('—'))
                        {
                            if (!card.Type_line.Substring(0, card.Type_line.IndexOf('—') - 1).ToLower().Equals(TypeBox.Text.ToLower()))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!card.Type_line.ToLower().Equals(TypeBox.Text.ToLower()))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        foreach (Cardface cardface in card.Card_faces)
                        {
                            if (cardface.Type_line.Contains('—'))
                            {
                                if (cardface.Type_line.Substring(0, cardface.Type_line.IndexOf('—') - 1).ToLower().Equals(TypeBox.Text.ToLower()))
                                {
                                    pass = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (cardface.Type_line.ToLower().Equals(TypeBox.Text.ToLower()))
                                {
                                    pass = true;
                                    break;
                                }
                            }
                        }

                        if (!pass)
                        {
                            return false;
                        }
                        else
                        {
                            pass = false;
                        }
                    }
                }
                else
                {
                    if (card.Card_faces is null)
                    {
                        if (card.Type_line.Contains('—'))
                        {
                            if (!Regex.IsMatch(card.Type_line.Substring(0, card.Type_line.IndexOf('—') - 1).ToLower(), TypeBox.Text.ToLower()))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!Regex.IsMatch(card.Type_line.ToLower(), TypeBox.Text.ToLower()))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        foreach (Cardface cardface in card.Card_faces)
                        {
                            if (cardface.Type_line.Contains('—'))
                            {
                                if (Regex.IsMatch(cardface.Type_line.Substring(0, cardface.Type_line.IndexOf('—') - 1).ToLower(), TypeBox.Text.ToLower()))
                                {
                                    pass = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (Regex.IsMatch(cardface.Type_line.ToLower(), TypeBox.Text.ToLower()))
                                {
                                    pass = true;
                                    break;
                                }
                            }
                        }

                        if (!pass)
                        {
                            return false;
                        }
                    }
                }

            }

            pass = false;
            if (!String.IsNullOrEmpty(SubtypeBox.Text))
            {
                if (exactsubtype)
                {
                    if (card.Card_faces is null)
                    {
                        if (card.Type_line.Contains('—'))
                        {
                            if (!card.Type_line.Substring(card.Type_line.IndexOf('—') + 2, card.Type_line.Length - (card.Type_line.IndexOf('—') + 2)).ToLower().Equals(SubtypeBox.Text.ToLower()))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        foreach (Cardface cardface in card.Card_faces)
                        {
                            if (cardface.Type_line.Contains('—'))
                            {
                                if (cardface.Type_line.Substring(cardface.Type_line.IndexOf('—') + 2, cardface.Type_line.Length - (cardface.Type_line.IndexOf('—') + 2)).ToLower().Equals(SubtypeBox.Text.ToLower()))
                                {
                                    pass = true;
                                    break;
                                }
                            }
                        }

                        if (!pass)
                        {
                            return false;
                        }
                        else
                        {
                            pass = false;
                        }
                    }
                }
                else
                {
                    if (card.Card_faces is null)
                    {
                        if (card.Type_line.Contains('—'))
                        {
                            if (!Regex.IsMatch(card.Type_line.Substring(card.Type_line.IndexOf('—') + 2, card.Type_line.Length - (card.Type_line.IndexOf('—') + 2)).ToLower(), SubtypeBox.Text.ToLower()))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        foreach (Cardface cardface in card.Card_faces)
                        {
                            if (cardface.Type_line.Contains('—'))
                            {
                                if (Regex.IsMatch(cardface.Type_line.Substring(cardface.Type_line.IndexOf('—') + 2, cardface.Type_line.Length - (cardface.Type_line.IndexOf('—') + 2)).ToLower(), SubtypeBox.Text.ToLower()))
                                {
                                    pass = true;
                                    break;
                                }
                            }
                        }

                        if (!pass)
                        {
                            return false;
                        }
                    }
                }
            }

            pass = false;
            if (!String.IsNullOrEmpty(KeywordBox.Text))
            {
                if (exactkeyword)
                {
                    foreach (string keyword in card.Keywords)
                    {
                        if (keyword.ToLower().Equals(KeywordBox.Text.ToLower()))
                        {
                            pass = true;
                            break;
                        }
                    }

                    if (!pass)
                    {
                        return false;
                    }
                }
                else
                {
                    foreach (string keyword in card.Keywords)
                    {
                        if (Regex.IsMatch(keyword.ToLower(), KeywordBox.Text.ToLower()))
                        {
                            pass = true;
                            break;
                        }
                    }

                    if (!pass)
                    {
                        return false;
                    }
                }
            }

            if (!String.IsNullOrEmpty(CMCBox.Text))
            {
                if (!card.Cmc.Equals(CMCBox.Text + ".0"))
                {
                    return false;
                }
            }

            pass = false;
            if (!String.IsNullOrEmpty(TextBox.Text))
            {
                if (card.Card_faces is null)
                {
                    if (String.IsNullOrEmpty(card.Oracle_text) || !Regex.IsMatch(card.Oracle_text.ToLower(), TextBox.Text))
                    {
                        return false;
                    }
                }
                else
                {
                    foreach (Cardface cardface in card.Card_faces)
                    {
                        if (!String.IsNullOrEmpty(cardface.Oracle_text) && Regex.IsMatch(cardface.Oracle_text.ToLower(), TextBox.Text))
                        {
                            pass = true;
                            break;
                        }
                    }

                    if (!pass)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool MainDisplay_Filter(Object item)
        {
            Card card = item as Card;
            return CheckCard(card);
        }

        private bool GroupDispaly_Filter(Object item)
        {
            Card card = (item as CardWithFilter).Thecard;
            return CheckCard(card);
        }

        private void Close_ButtonClick(object sender, RoutedEventArgs e)
        {
            Program.CloseApp();
            System.Windows.Application.Current.Shutdown();
        }

        private void RemoveCard_Click(object sender, RoutedEventArgs e)
        {
            //Remove Card From DB
            if(!(selectedcard is null))
            {
                if(RemoveBox.Text.Length > 0 && Regex.IsMatch(RemoveBox.Text, @"^[0-9]+$") && Convert.ToInt32(RemoveBox.Text) > 0 && Convert.ToInt32(RemoveBox.Text) <= selectedcard.Amount)
                {
                    List<CardList> AffectedLists = new List<CardList>();
                    List<CardInList> AffectedCards = new List<CardInList>();
                    string affeclists = "";
                    string affectcards = "";
                    foreach(CardList cardlist in Program.cardlists)
                    {
                        foreach(CardInList cardinlist in cardlist.TheList)
                        {
                            if(cardinlist.TheCard == selectedcard)
                            {
                                if(cardinlist.AmountInList > selectedcard.Amount - Convert.ToInt32(RemoveBox.Text))
                                {
                                    AffectedLists.Add(cardlist);
                                    affeclists = affeclists + "- " + cardlist.Name + Environment.NewLine;
                                    AffectedCards.Add(cardinlist);
                                    affectcards = affectcards + "- " + cardlist.Name + ", " + cardinlist.TheCard.Name + " " + cardinlist.AmountInList + " -> " + (selectedcard.Amount - Convert.ToInt32(RemoveBox.Text)) + Environment.NewLine;
                                }
                                break;
                            }
                        }
                    }

                    if (AffectedLists.Count > 0)
                    {
                        if (MessageBox.Show("Card Is In The Following Lists And Will Be Affected. Do You Still Wish To Remove?" + Environment.NewLine + affeclists, "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                        {
                            return;
                        }

                        for(int i = 0; i < AffectedCards.Count; i++)
                        {
                            if(selectedcard.Amount - Convert.ToInt32(RemoveBox.Text) == 0)
                            {
                                AffectedLists[i].TheList.Remove(AffectedCards[i]);
                            }
                            else
                            {
                                AffectedCards[i].AmountInList = selectedcard.Amount - Convert.ToInt32(RemoveBox.Text);
                            }
                        }

                        MessageBox.Show(affectcards);
                    }

                    if(selectedcard.Amount - Convert.ToInt32(RemoveBox.Text) == 0)
                    {
                        //remove card and changed ID
                        Card.dbid = Card.dbid - 1;
                        for(int i = Program.allcards.IndexOf(selectedcard) + 1; i < Program.allcards.Count; i++)
                        {
                            Program.allcards[i].Database_id = Program.allcards[i].Database_id - 1;
                        }
                        Program.allcards.Remove(selectedcard);
                        Program.cardsbycolorandcmc.Remove(selectedcard);

                        int ind = -1;
                        foreach (List<CardWithFilter> thelist in Program.alllistofcardsbyfilter)
                        {
                            ind = -1;
                            for (int i = 0; i < thelist.Count; i++)
                            {
                                if (thelist[i].Thecard == selectedcard)
                                {
                                    ind = i;
                                    break;
                                }
                            }

                            if (ind != -1)
                            {
                                thelist.RemoveAt(ind);
                            }
                        }

                        ((CardDisp.ItemsSource) as ObservableCollection<Card>).Remove(selectedcard);
                        MessageBox.Show("Card Removed");
                    }
                    else
                    {
                        selectedcard.Amount = selectedcard.Amount - Convert.ToInt32(RemoveBox.Text);
                        MessageBox.Show("Card Amount Changed");
                    }

                    CardImg.Source = null;
                    CardImgF1.Source = null;
                    CardImgF2.Source = null;

                    RemoveBox.Text = "";

                    CardDisp.Items.Refresh();
                    CardDisp.UnselectAll();

                }
                else
                {
                    MessageBox.Show("Enter Valid Number");
                }
            }
            else
            {
                MessageBox.Show("Select Card");
            }
        }

        //List stuff

        private void ListDispaly_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);

            if(!(lbi is null))
            {
                string selection = lbi.Content.ToString()[0].ToString();

                CardImg.Source = null;
                CardImgF1.Source = null;
                CardImgF2.Source = null;

                cardidsincurlist.Clear();
                ListCardDispCreate.ItemsSource = null;
                selectedlist = null;

                ListNameBox.Clear();
                ListDescBox.Clear();
                CardDBIDBox.Clear();
                CardAmountBox.Clear();

                CardDispCreateList.UnselectAll();

                ListName.Visibility = Visibility.Visible;
                ListNameBox.Visibility = Visibility.Visible;
                ListDes.Visibility = Visibility.Visible;
                ListDescBox.Visibility = Visibility.Visible;
                ListCardID.Visibility = Visibility.Visible;
                ListCardAmout.Visibility = Visibility.Visible;
                CardDBIDBox.Visibility = Visibility.Visible;
                CardAmountBox.Visibility = Visibility.Visible;
                CreateSaveBtn.Visibility = Visibility.Visible;
                AddCardToListBtn.Visibility = Visibility.Visible;
                UpdateCardInListBtn.Visibility = Visibility.Visible;
                RemoveCardListBtn.Visibility = Visibility.Visible;
                DeleteListBtn.Visibility = Visibility.Visible;
                ListCardDispCreate.Visibility = Visibility.Visible;

                ArchTapBtn.Visibility = Visibility.Hidden;
                StarKingBtn.Visibility = Visibility.Hidden;
                TCGPBtn.Visibility = Visibility.Hidden;
                LoadBtn.Visibility = Visibility.Hidden;

                LoadListSelc.Visibility = Visibility.Hidden;
                LoadListBtn.Visibility = Visibility.Hidden;
                LoadListTextBox.Visibility = Visibility.Hidden;

                selectedloadablefile = "";

                if (selection.Equals("1"))
                {
                    //View All Lists
                    CreateSaveBtn.Content = "Save";
                    ListCardDispCreate.Height = 219;
                    Thickness margin = ListCardDispCreate.Margin;
                    margin.Left = 10;
                    margin.Top = 50;
                    margin.Left = 10;
                    ListCardDispCreate.Margin = margin;
                    ListSelc.Visibility = Visibility.Visible;
                    dispalypage = true;
                    createlist = false;
                    ListSelc.Items.Refresh();
                    ListSelc.SelectedIndex = -1;
                }
                else if (selection.Equals("2"))
                {
                    //Create List
                    CreateSaveBtn.Content = "Create";
                    ListSelc.Visibility = Visibility.Hidden;
                    ListCardDispCreate.Height = 259;
                    Thickness margin = ListCardDispCreate.Margin;
                    margin.Left = 10;
                    margin.Top = 10;
                    margin.Left = 10;
                    ListCardDispCreate.Margin = margin;
                    dispalypage = false;
                    createlist = true;
                }
                else if (selection.Equals("3"))
                {
                    //Print List
                    ListName.Visibility = Visibility.Hidden;
                    ListNameBox.Visibility = Visibility.Hidden;
                    ListDes.Visibility = Visibility.Hidden;
                    ListDescBox.Visibility = Visibility.Hidden;
                    ListCardID.Visibility = Visibility.Hidden;
                    ListCardAmout.Visibility = Visibility.Hidden;
                    CardDBIDBox.Visibility = Visibility.Hidden;
                    CardAmountBox.Visibility = Visibility.Hidden;
                    CreateSaveBtn.Visibility = Visibility.Hidden;
                    AddCardToListBtn.Visibility = Visibility.Hidden;
                    UpdateCardInListBtn.Visibility = Visibility.Hidden;
                    RemoveCardListBtn.Visibility = Visibility.Hidden;
                    DeleteListBtn.Visibility = Visibility.Hidden;

                    ArchTapBtn.Visibility = Visibility.Visible;
                    StarKingBtn.Visibility = Visibility.Visible;
                    TCGPBtn.Visibility = Visibility.Visible;
                    LoadBtn.Visibility = Visibility.Visible;

                    ListCardDispCreate.Height = 219;
                    Thickness margin = ListCardDispCreate.Margin;
                    margin.Left = 10;
                    margin.Top = 50;
                    margin.Left = 10;
                    ListCardDispCreate.Margin = margin;
                    ListSelc.Visibility = Visibility.Visible;
                    createlist = false;
                    ListSelc.Items.Refresh();
                    ListSelc.SelectedIndex = -1;

                }
                else if (selection.Equals("4"))
                {
                    //Load List
                    string[] files = Directory.GetFiles(filedir);
                    loadablefilepaths.Clear();
                    foreach (string file in files)
                    {
                        string filename = System.IO.Path.GetFileName(file);

                        if (filename.Contains("Load.txt"))
                        {
                            loadablefilepaths.Add(new LoadableFile(file));
                        }
                    }

                    ListName.Visibility = Visibility.Hidden;
                    ListNameBox.Visibility = Visibility.Hidden;
                    ListDes.Visibility = Visibility.Hidden;
                    ListDescBox.Visibility = Visibility.Hidden;
                    ListCardID.Visibility = Visibility.Hidden;
                    ListCardAmout.Visibility = Visibility.Hidden;
                    CardDBIDBox.Visibility = Visibility.Hidden;
                    CardAmountBox.Visibility = Visibility.Hidden;
                    CreateSaveBtn.Visibility = Visibility.Hidden;
                    AddCardToListBtn.Visibility = Visibility.Hidden;
                    UpdateCardInListBtn.Visibility = Visibility.Hidden;
                    RemoveCardListBtn.Visibility = Visibility.Hidden;
                    DeleteListBtn.Visibility = Visibility.Hidden;
                    ListSelc.Visibility = Visibility.Hidden;
                    ListCardDispCreate.Visibility = Visibility.Hidden;

                    LoadListSelc.Visibility = Visibility.Visible;
                    LoadListBtn.Visibility = Visibility.Visible;
                    LoadListTextBox.Visibility = Visibility.Visible;

                    LoadListSelc.Items.Refresh();
                    LoadListSelc.SelectedIndex = -1;
                    LoadListTextBox.Text = "";
                }
                else if (selection.Equals("5"))
                {
                    //Switch To Card Display
                    ListViewSelect.UnselectAll();

                    CardViewSelet.Visibility = Visibility.Visible;

                    RemoveCardBtn.Visibility = Visibility.Visible;
                    Removetxt.Visibility = Visibility.Visible;
                    RemoveBox.Visibility = Visibility.Visible;

                    ListViewSelect.Visibility = Visibility.Hidden;
                    ListSelc.Visibility = Visibility.Hidden;
                    ListCardDispCreate.Visibility = Visibility.Hidden;
                    CardDispCreateList.Visibility = Visibility.Hidden;
                    ListName.Visibility = Visibility.Hidden;
                    ListNameBox.Visibility = Visibility.Hidden;
                    ListDes.Visibility = Visibility.Hidden;
                    ListDescBox.Visibility = Visibility.Hidden;
                    ListCardID.Visibility = Visibility.Hidden;
                    ListCardAmout.Visibility = Visibility.Hidden;
                    CardDBIDBox.Visibility = Visibility.Hidden;
                    CardAmountBox.Visibility = Visibility.Hidden;
                    CreateSaveBtn.Visibility = Visibility.Hidden;
                    AddCardToListBtn.Visibility = Visibility.Hidden;
                    UpdateCardInListBtn.Visibility = Visibility.Hidden;
                    RemoveCardListBtn.Visibility = Visibility.Hidden;
                    DeleteListBtn.Visibility = Visibility.Hidden;

                    CardDisp.Visibility = Visibility.Visible;
                    theitemsource = CardDisp.ItemsSource;
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(theitemsource);
                    view.Filter = MainDisplay_Filter;
                    currentlistview = CardDisp;
                }
            }
            

        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Select List
            if(e.AddedItems.Count != 0)
            {
                ListCardDispCreate.ItemsSource = (e.AddedItems[0] as CardList).TheList;
                selectedlist = e.AddedItems[0] as CardList;

                ListNameBox.Text = selectedlist.Name;
                ListDescBox.Text = selectedlist.Description;

                cardidsincurlist.Clear();

                foreach (CardInList cil in selectedlist.TheList)
                {
                    cardidsincurlist.Add(cil.TheCard.Database_id);
                }
            }

            CardDBIDBox.Clear();
            CardAmountBox.Clear();
        }

        private void ListCardDisp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Select Card From List
            CardDispCreateList.UnselectAll();
            if (e.AddedItems.Count > 0)
            {
                Card selc = (e.AddedItems[0] as CardInList).TheCard;

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
        }

        private void CardDispListSel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Select Card From List
            ListCardDispCreate.UnselectAll();
            if (e.AddedItems.Count > 0)
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
        }

        private void CreateList_Click(object sender, RoutedEventArgs e)
        {
            //Create New List
            if (selectedlist is null && createlist == false)
            {
                MessageBox.Show("Select List First");
            }
            else if (!(ListNameBox.Text is null) && ListNameBox.Text.Length > 0 && Regex.IsMatch(ListNameBox.Text + ".txt", @"^[a-zA-Z0-9](?:[a-zA-Z0-9 ._-]*[a-zA-Z0-9])?\.[a-zA-Z0-9_-]+$"))
            {
                if (createlist && !dispalypage)
                {
                    if (!(ListDescBox.Text is null))
                    {
                        selectedlist = new CardList(ListNameBox.Text, ListDescBox.Text);
                        Program.cardlists.Add(selectedlist);
                    }
                    else
                    {
                        selectedlist = new CardList(ListNameBox.Text, "");
                        Program.cardlists.Add(selectedlist);
                    }
                    CreateSaveBtn.Content = "Save";
                    createlist = false;
                    ListCardDispCreate.ItemsSource = selectedlist.TheList;
                }
                else
                {
                    selectedlist.Name = ListNameBox.Text;

                    if (!(ListDescBox.Text is null))
                    {
                        selectedlist.Description = ListDescBox.Text;
                    }

                    if(!(ListSelc is null))
                    {
                        ListSelc.Items.Refresh();
                    }
                }
            }
            else
            {
                if (ListNameBox.Text is null || ListNameBox.Text.Length == 0)
                {
                    MessageBox.Show("List Name Can Not Be Empty");
                }
                else if (!(Regex.IsMatch(ListNameBox.Text + ".txt", @"^[a-zA-Z0-9](?:[a-zA-Z0-9 ._-]*[a-zA-Z0-9])?\.[a-zA-Z0-9_-]+$")))
                {
                    MessageBox.Show("List Name Is Not A Valid File Name");
                }
            }
        }

        private void DeleteList_Click(object sender, RoutedEventArgs e)
        {
            //Delete Selected List
            if(selectedlist is null && dispalypage)
            {
                MessageBox.Show("Select List First");
            }
            else if(selectedlist is null && !dispalypage)
            {
                MessageBox.Show("Create List First");
            }
            else
            {
                if(dispalypage)
                {
                    ListSelc.SelectedIndex = -1;
                }
                else
                {
                    createlist = true;
                    CreateSaveBtn.Content = "Create";
                }

                ListCardDispCreate.ItemsSource = null;
                Program.cardlists.Remove(selectedlist);
                selectedlist = null;
                ListNameBox.Text = "";
                ListDescBox.Text = "";
                CardDBIDBox.Text = "";
                CardAmountBox.Text = "";
                CardImg.Source = null;
                CardImgF1.Source = null;
                CardImgF2.Source = null;
                ListSelc.Items.Refresh();
            }
        }

        private bool AddUpListCheck()
        {
            //Does Check For Adding/Updating/Removing Card In A List
            if(selectedlist is null && dispalypage)
            {
                MessageBox.Show("Select List First");
            }
            else if (!createlist)
            {
                if (!(CardDBIDBox.Text is null) && !(CardAmountBox.Text is null) && Regex.IsMatch(CardDBIDBox.Text, @"^[0-9]+$") && Regex.IsMatch(CardAmountBox.Text, @"^[0-9]+$"))
                {
                    if (Convert.ToInt32(CardDBIDBox.Text) >= 0 && Convert.ToInt32(CardDBIDBox.Text) <= Program.allcards.Count)
                    {
                        if (Convert.ToInt32(CardAmountBox.Text) > 0 && Convert.ToInt32(CardAmountBox.Text) <= Program.allcards[Convert.ToInt32(CardDBIDBox.Text)].Amount)
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Card Amount Is Not Valid");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Card ID Is Not Valid");
                    }
                }
                else
                {
                    MessageBox.Show("ID And Amount Must Be Numbers");
                }
            }
            else
            {
                MessageBox.Show("Create List First");
            }

            return false;
        }

        private void AddToList_Click(object sender, RoutedEventArgs e)
        {
            //Add Card To List
            if(AddUpListCheck())
            {
                if (cardidsincurlist.Contains(Program.allcards[Convert.ToInt32(CardDBIDBox.Text)].Database_id))
                {
                    MessageBox.Show("Card Already In List");
                }
                else
                {
                    selectedlist.TheList.Add(new CardInList { TheCard = Program.allcards[Convert.ToInt32(CardDBIDBox.Text)], AmountInList = Convert.ToInt32(CardAmountBox.Text) });
                    cardidsincurlist.Add(Program.allcards[Convert.ToInt32(CardDBIDBox.Text)].Database_id);
                    CardAmountBox.Text = "";
                    CardDBIDBox.Text = "";
                    ListCardDispCreate.Items.Refresh();
                }
            }
        }

        private void UpdateCardInList_Click(object sender, RoutedEventArgs e)
        {
            //Update Card In List
            if(AddUpListCheck())
            {
                if (cardidsincurlist.Contains(Program.allcards[Convert.ToInt32(CardDBIDBox.Text)].Database_id))
                {
                    selectedlist.TheList[cardidsincurlist.IndexOf(Program.allcards[Convert.ToInt32(CardDBIDBox.Text)].Database_id)].AmountInList = Convert.ToInt32(CardAmountBox.Text);
                    ListCardDispCreate.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Card Not In List");
                }
            }
        }

        private void RemoveCardInList_Click(object sender, RoutedEventArgs e)
        {
            //Remove Card From List
            if(AddUpListCheck())
            {
                if (cardidsincurlist.Contains(Program.allcards[Convert.ToInt32(CardDBIDBox.Text)].Database_id))
                {
                    if(selectedlist.TheList[cardidsincurlist.IndexOf(Program.allcards[Convert.ToInt32(CardDBIDBox.Text)].Database_id)].AmountInList >= Convert.ToInt32(CardAmountBox.Text))
                    {
                        if(selectedlist.TheList[cardidsincurlist.IndexOf(Program.allcards[Convert.ToInt32(CardDBIDBox.Text)].Database_id)].AmountInList == Convert.ToInt32(CardAmountBox.Text))
                        {
                            selectedlist.TheList.RemoveAt(cardidsincurlist.IndexOf(Program.allcards[Convert.ToInt32(CardDBIDBox.Text)].Database_id));
                            cardidsincurlist.Remove(Program.allcards[Convert.ToInt32(CardDBIDBox.Text)].Database_id);
                        }
                        else
                        {
                            selectedlist.TheList[cardidsincurlist.IndexOf(Program.allcards[Convert.ToInt32(CardDBIDBox.Text)].Database_id)].AmountInList -= Convert.ToInt32(CardAmountBox.Text);
                        }
                        CardAmountBox.Text = "";
                        CardDBIDBox.Text = "";
                        ListCardDispCreate.Items.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("Card Amount Is Not Valid");
                    }
                }
                else
                {
                    MessageBox.Show("Card Not In List");
                }
            }
        }

        private void ArchTap_Click(object sender, RoutedEventArgs e)
        {
            //Printing List
            if(selectedlist is null)
            {
                MessageBox.Show("Select List First");
            }
            else
            {
                string docpath = Environment.CurrentDirectory + "\\Files";
                using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(docpath, selectedlist.Name + "DeckBuild.txt"), false))
                {
                    outputFile.WriteLine("Name: " + selectedlist.Name);
                    outputFile.WriteLine("");
                    outputFile.WriteLine("Description: " + selectedlist.Description);
                    outputFile.WriteLine("");
                    outputFile.WriteLine("");
                    List<CardInList> cardsinlist = selectedlist.TheList;
                    foreach(CardInList cil in cardsinlist)
                    {
                        outputFile.WriteLine(cil.AmountInList + "x " + cil.TheCard.Name + " (" + cil.TheCard.Set + ")");
                    }
                }

                MessageBox.Show("Archidekt/Tappedout File Printed to 'Files' Directory");
            }
        }

        private void StrCitCrdKin_Click(object sender, RoutedEventArgs e)
        {
            //Printing List
            if (selectedlist is null)
            {
                MessageBox.Show("Select List First");
            }
            else
            {
                string docpath = Environment.CurrentDirectory + "\\Files";
                using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(docpath, selectedlist.Name + "StarKingdom.txt"), false))
                {
                    outputFile.WriteLine("Name: " + selectedlist.Name);
                    outputFile.WriteLine("");
                    outputFile.WriteLine("Description: " + selectedlist.Description);
                    outputFile.WriteLine("");
                    outputFile.WriteLine("");
                    List<CardInList> cardsinlist = selectedlist.TheList;
                    foreach (CardInList cil in cardsinlist)
                    {
                        outputFile.WriteLine(cil.AmountInList + "x " + cil.TheCard.Name);
                    }
                }

                MessageBox.Show("StarCity/CardKingdom File Printed to 'Files' Directory");
            }
        }

        private void TCGP_Click(object sender, RoutedEventArgs e)
        {
            //Printing List
            if (selectedlist is null)
            {
                MessageBox.Show("Select List First");
            }
            else
            {
                string docpath = Environment.CurrentDirectory + "\\Files";
                using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(docpath, selectedlist.Name + "TCGPlayer.txt"), false))
                {
                    outputFile.WriteLine("Name: " + selectedlist.Name);
                    outputFile.WriteLine("");
                    outputFile.WriteLine("Description: " + selectedlist.Description);
                    outputFile.WriteLine("");
                    outputFile.WriteLine("");
                    List<CardInList> cardsinlist = selectedlist.TheList;
                    foreach (CardInList cil in cardsinlist)
                    {
                        outputFile.WriteLine(cil.AmountInList + "x " + cil.TheCard.Name + " [" + cil.TheCard.Set + "]");
                    }
                }

                MessageBox.Show("TCGPlayer File Printed to 'Files' Directory");
            }
        }

        private void Loadable_Click(object sender, RoutedEventArgs e)
        {
            //Printing List
            if (selectedlist is null)
            {
                MessageBox.Show("Select List First");
            }
            else
            {
                string docpath = Environment.CurrentDirectory + "\\Files";
                using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(docpath, selectedlist.Name + "Load.txt"), false))
                {
                    outputFile.WriteLine(selectedlist.Name);
                    outputFile.WriteLine(selectedlist.Description);
                    List<CardInList> cardsinlist = selectedlist.TheList;
                    foreach (CardInList cil in cardsinlist)
                    {
                        outputFile.WriteLine(cil.TheCard.Name + "_" + cil.TheCard.Set + "_" + cil.TheCard.Collector_number + "_" + cil.TheCard.Printing + "_" + cil.AmountInList);
                    }
                }

                MessageBox.Show("Loadable File Printed to 'Files' Directory");
            }
        }

        private void LoadList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.AddedItems.Count != 0)
            {
                selectedloadablefile = (e.AddedItems[0] as LoadableFile).Path;
               
            }

        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            //Load List And Add Put It With The Other Lists
            if(selectedloadablefile.Equals(""))
            {
                MessageBox.Show("Select List First");
            }
            else
            {
                string[] lines = System.IO.File.ReadAllLines(selectedloadablefile);
                CardList loadlist = new CardList(lines[0],lines[1]);
                LoadListTextBox.Text = "";
                for (int i = 2; i < lines.Length; i++)
                {
                    string[] cardelements = lines[i].Split("_");
                    string cardname = cardelements[0];
                    string cardset = cardelements[1];
                    string cardcolnum = cardelements[2];
                    string cardprint = cardelements[3];
                    string cardnum = cardelements[4];
                    int min = 0;
                    int max = Program.allcards.Count - 1;
                    int index = 0;
                    bool found = false;
                    Card card = null;
                    bool canload = false;
                    while (min <= max)
                    {
                        int mid = (min + max) / 2;
                        if (cardname.Equals(Program.allcards[mid].Name))
                        {
                            index = mid;
                            if (cardset.Equals(Program.allcards[mid].Set) && cardcolnum.Equals(Program.allcards[mid].Collector_number) && cardprint.Equals(Program.allcards[mid].Printing))
                            {
                                card = Program.allcards[mid];
                                found = true;
                                break;
                            }
                            else
                            {
                                for (int j = index + 1; j < Program.allcards.Count; j++)
                                {
                                    if (cardname.Equals(Program.allcards[j].Name))
                                    {
                                        if (cardset.Equals(Program.allcards[j].Set) && cardcolnum.Equals(Program.allcards[j].Collector_number) && cardprint.Equals(Program.allcards[j].Printing))
                                        {
                                            card = Program.allcards[j];
                                            found = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                if (found)
                                {
                                    break;
                                }
                                for (int j = index - 1; j >= 0; j--)
                                {
                                    if (cardname.Equals(Program.allcards[j].Name))
                                    {
                                        if (cardset.Equals(Program.allcards[j].Set) && cardcolnum.Equals(Program.allcards[j].Collector_number) && cardprint.Equals(Program.allcards[j].Printing))
                                        {
                                            card = Program.allcards[j];
                                            found = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        else if (cardname.CompareTo(Program.allcards[mid].Name) < 0)
                        {
                            max = mid - 1;
                        }
                        else
                        {
                            min = mid + 1;
                        }
                    }

                    if (found)
                    {
                        if (Convert.ToInt32(cardnum) > card.Amount)
                        {
                            //card in load is greater amount then card in db
                            if (MessageBox.Show(card.Special_name + " Has More In The List" + " (" + cardnum + ")" + " Then In The Database (" + card.Amount + ")." + Environment.NewLine + "Skip Loading Card or Cancel Load", "Load Error", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                            {
                                //cancel load
                                canload = true;
                            }
                        }
                        else
                        {
                            //card fine
                            if(LoadListTextBox.Text.Equals(""))
                            {
                                LoadListTextBox.Text = card.Special_name + " Loaded";
                            }
                            else
                            {
                                LoadListTextBox.Text = LoadListTextBox.Text + Environment.NewLine + card.Special_name + " Loaded";
                            }
                            loadlist.TheList.Add(new CardInList { TheCard = card, AmountInList = Convert.ToInt32(cardnum) });
                            //loadlist.NumOfCard.Add(Convert.ToInt32(cardnum));
                        }
                    }
                    else
                    {
                        //card not in db
                        if (MessageBox.Show(cardname + " Is Not In The Database." + Environment.NewLine +"Skip Loading Card or Cancel Load", "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                        {
                            canload = true;
                        }
                    }

                    if (canload)
                    {
                        LoadListTextBox.Text = "";
                        return;
                    }
                }
                LoadListTextBox.Text = "List Loaded" + Environment.NewLine + LoadListTextBox.Text;
                Program.cardlists.Add(loadlist);
            }
        }
    }
}
