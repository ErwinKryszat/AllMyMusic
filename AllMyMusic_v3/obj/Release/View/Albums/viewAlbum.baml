   M S B A M L   `   `   `  ���� E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View      AllMyMusic.View.viewAlbum   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ,vwclr-namespace:AllMyMusic.View     -   $��
viewAlbum1��  � 	Resources   �� MergedDictionaries �� Source$   AllMyMusic.View.ImageConverter   imagePathToImageConverter*  $AllMyMusic.View.RatingImageConverter  ratingImageConverter1+   %AllMyMusic.View.CountryImageConverter  countryImageConverter1  headlineVisibility��  �� $. (../../ResourceDictionaries/MainSkin.xaml?��� $+ %../../Styles/NoChromeButtonStyle.xaml?��� $6 0../../ControlTemplates/RatingSliderTemplate.xaml?�%<   &	        &	      &	 
     &	            �� .���   PagerBackground#���   � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+���   HeadlineBackground#���  $��0��._���   HeadlineText#���  $��36(�$
��Bold"�$��Center=�����  �� Path$ HeadlineAlbum� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     ShadowDepth$ 2Q�   	Direction$	 315Q�   Color$ Black��	   Opacity$		 1.0Q�
   
BlurRadius$
 1Q��� $��1��$
��Auto=�$
��Auto=�.���   PanelBackground#���   �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��    bandColumnVisibility 	 ProxyElement � DataContext��%<   &	       &		      �� �  �� $ SongsViewModel�.+��� $y�	Collapsed=�#��[	 $   AllMyMusic.View.CustomDataGrid  -     Name$ dataGridTracks��$   AllMyMusic.View.CustomDataGrid   CellEditEnding$# dataGridTracks_CellEditEnding��&   System.Windows.Controls.DataGrid   
IsReadOnly .  $	��200��$��
10,10,10,0q� �� $ SongsViewModel�   SelectedItemsList �� $ SelectedSongs� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=����� $ Songs�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,	  &System.Windows.Controls.DataGridColumn 	  Header$ Track�� 	  
IsReadOnly .  1
  +System.Windows.Controls.DataGridBoundColumn 
  Binding �� *Track+  $ BandName�� .   �� *
BandName+ 	  
Visibility �� $# DataContext.BandColumnVisible� �� Source# [	  �� 	Converter# [   $ 	SongTitle�� .   �� *	SongTitle+  $ Duration�� .  �� *
Duration+4  .System.Windows.Controls.DataGridTemplateColumn  $ Rating��   CellTemplate �� .R��� ���� $ Rating�# [    CellEditingTemplate �� .R�"   AllMyMusic.View.RatingSlider    Name$ ratingSlider1�� 
 RatingSliderTemplate#��[
 $��16��   ForegroundRectColor# �     TagValue �� $ Rating���  �� Orientation$ 
Horizontal=�$��1��.+�� $��10q�  AlbumItemBackground#���   �� $
��Auto �.+��� ���	#49FFFFFF$��2,2,0,0q��� ���	#49000000$��0,0,2,2q���   NoChromeButtonStyle#��[ ���� $ CoverImageCommand�.���� $��5q�$	��200��$	��200������ $ Album.FrontImageFullpath�# [  ���� $ SleeveImageSize����� $ SleeveImageSize��� .+��� #��[ ���� $ CountryImageCommand�.���� $��30��$��60��$
��Left=�$	��Top=�$��10q����� $ FlagPath�# [ ���� $# DataContext.CountryFlagHeight� �� RelativeSource �� *FindAncestor+  �� AncestorType   AllMyMusic.View.viewAlbum"    �� $ Country�! � ToolTip! �� ���� $ Country�&    AllMyMusic.View.viewAlbumDetails  $��	0,5,10,10q� �� $ AlbumDetails�"   AllMyMusic.View.viewCommands  $��2��$��	20,5,20,5q� �� 