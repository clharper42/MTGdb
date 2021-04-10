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
        //list stuff
        private List<int> cardidsincurlist = new List<int>(); //clear out on switch
        private CardList selectedlist;
        private bool createlist = false;
        private bool dispalypage = true;
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
            CardDisp.ItemsSource = Program.allcards;
            CardDispPrice.ItemsSource = Program.allcards;

            theitemsource = CardDisp.ItemsSource;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(theitemsource);
            view.Filter = MainDisplay_Filter;


            //list stuff
            ListSelc.ItemsSource = Program.cardlists;
            CardDispCreateList.ItemsSource = Program.allcards;

            //theitemsource = CardDispCreateList.ItemsSource;
            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(theitemsource);
            //view.Filter = MainDisplay_Filter;

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

                CardDispColor.ItemsSource = null;
                CardDispColorType.ItemsSource = null;
                CardDispColorSubtype.ItemsSource = null;
                CardDispColorCMC.ItemsSource = null;
                CardDispColorKeyword.ItemsSource = null;

                if (selection.Equals("1"))
                {
                    CardDisp.Visibility = Visibility.Visible;
                    theitemsource = CardDisp.ItemsSource;
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(theitemsource);
                    view.Filter = MainDisplay_Filter;
                }
                else if (selection.Equals("2"))
                {
                    //CardDispColor.Visibility = Visibility.Visible;
                    //CollectionView view = (CollectionView)new CollectionViewSource { Source = Program.cardsbycolorandcmc }.View;
                    //PropertyGroupDescription groupDescription = new PropertyGroupDescription("Colorstring");
                    //view.GroupDescriptions.Add(groupDescription);
                    //CardDispColor.ItemsSource = view;

                    CardDispColor.Visibility = Visibility.Visible;
                    CollectionView view = (CollectionView)new CollectionViewSource { Source = Program.cardsbycolorandcmc }.View;
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Colorstring");
                    view.GroupDescriptions.Add(groupDescription);
                    CardDispColor.ItemsSource = view;

                    theitemsource = CardDispColor.ItemsSource;
                    view.Filter = MainDisplay_Filter;
                }
                else if (selection.Equals("3"))
                {
                    CardDispColorType.Visibility = Visibility.Visible;
                    CollectionView view = (CollectionView)new CollectionViewSource { Source = Program.cardsbytype }.View;
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Coloridfilter");
                    view.GroupDescriptions.Add(groupDescription);
                    CardDispColorType.ItemsSource = view;

                    theitemsource = CardDispColorType.ItemsSource;
                    view.Filter = GroupDispaly_Filter;
                }
                else if (selection.Equals("4"))
                {
                    CardDispColorSubtype.Visibility = Visibility.Visible;
                    CollectionView view = (CollectionView)new CollectionViewSource { Source = Program.cardsbysubtype }.View;
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Coloridfilter");
                    view.GroupDescriptions.Add(groupDescription);
                    CardDispColorSubtype.ItemsSource = view;

                    theitemsource = CardDispColorSubtype.ItemsSource;
                    view.Filter = GroupDispaly_Filter;
                }
                else if (selection.Equals("5"))
                {
                    CardDispColorCMC.Visibility = Visibility.Visible;
                    CollectionView view = (CollectionView)new CollectionViewSource { Source = Program.cardsbycolorandcmc }.View;
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Coloridcmc");
                    view.GroupDescriptions.Add(groupDescription);
                    CardDispColorCMC.ItemsSource = view;

                    theitemsource = CardDispColorCMC.ItemsSource;
                    view.Filter = MainDisplay_Filter;
                }
                else if (selection.Equals("6"))
                {
                    CardDispColorKeyword.Visibility = Visibility.Visible;
                    CollectionView view = (CollectionView)new CollectionViewSource { Source = Program.cardsbykeyword }.View;
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Coloridfilter");
                    view.GroupDescriptions.Add(groupDescription);
                    CardDispColorKeyword.ItemsSource = view;

                    theitemsource = CardDispColorKeyword.ItemsSource;
                    view.Filter = GroupDispaly_Filter;
                }
                else if (selection.Equals("7"))
                {
                    CardDispPrice.Visibility = Visibility.Visible;
                    theitemsource = CardDispPrice.ItemsSource;
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(theitemsource);
                    view.Filter = MainDisplay_Filter;
                }
                else if (selection.Equals("8"))
                {
                    CardViewSelet.UnselectAll();

                    CardViewSelet.Visibility = Visibility.Hidden;
                    ListViewSelect.Visibility = Visibility.Visible;

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

        private void W_CheckedEvent(object sender, RoutedEventArgs e)
        {
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
                colorsfil = colorsfil.Remove(colorsfil.IndexOf("W"));
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void U_CheckedEvent(object sender, RoutedEventArgs e)
        {
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
                colorsfil = colorsfil.Remove(colorsfil.IndexOf("U"));
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void B_CheckedEvent(object sender, RoutedEventArgs e)
        {
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
                colorsfil = colorsfil.Remove(colorsfil.IndexOf("B"));
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void R_CheckedEvent(object sender, RoutedEventArgs e)
        {
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
                colorsfil = colorsfil.Remove(colorsfil.IndexOf("R"));
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void G_CheckedEvent(object sender, RoutedEventArgs e)
        {
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
                colorsfil = colorsfil.Remove(colorsfil.IndexOf("G"));
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void C_CheckedEvent(object sender, RoutedEventArgs e)
        {
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
                colorsfil = colorsfil.Remove(colorsfil.IndexOf("C"));
            }
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }


        private void Contains_CheckedEvent(object sender, RoutedEventArgs e)
        {
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
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void Type_Textchanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void Subtype_Textchanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void Keyword_Textchanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void CMC_Textchanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private void Textbox_Textchanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(theitemsource).Refresh();
        }

        private bool CheckCard(Card card)
        {
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
                else if (colorsfil.Contains("C") && card.Color_identity.Count > 0)
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

                if (selection.Equals("1"))
                {
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
                    ListViewSelect.UnselectAll();

                    CardViewSelet.Visibility = Visibility.Visible;

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
                }
            }
            

        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ListCardDisp.ItemsSource = (e.AddedItems[0] as CardList).TheList;

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
            //ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);
            //string cardsel = lbi.Content.ToString();
            //CardImg.Source = new BitmapImage(new Uri(Program.allcards[Convert.ToInt32(cardsel[cardsel.Length - 1].ToString())].Image_uris["normal"]));
        }

        private void CreateList_Click(object sender, RoutedEventArgs e)
        {
            //have seclected list stored here
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
    }
}
