   M S B A M L   `   `   `  ����E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View  5          AllMyMusic.View.viewAlbum   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,vwclr-namespace:AllMyMusic.View     5      -   5        $��
viewAlbum1��5        � 	Resources  5       �� MergedDictionaries �� Source$   AllMyMusic.View.ImageConverter   imagePathToImageConverter*  $AllMyMusic.View.RatingImageConverter  ratingImageConverter1+   %AllMyMusic.View.CountryImageConverter  countryImageConverter1  headlineVisibility�� 5   
    5      �� 5      $. (../../ResourceDictionaries/MainSkin.xaml?�6%   �� 5      $+ %../../Styles/NoChromeButtonStyle.xaml?�6%   �� 5      $6 0../../ControlTemplates/RatingSliderTemplate.xaml?�6%   %t   &	        &	      &	 &     &	 9       5      6      5      6&     5      6'   �� 5      6+   5      .��5      �   PagerBackground#���  6    � RowDefinitions 5    
   �� 5!      $
��Auto �6   �� 5"      $��* �6   �� 5#      $
��Auto �6   .+�5&   
   ��   HeadlineBackground#���  6   $��0��6E   ._�5'      ��   HeadlineText#���  5)      $��36(�5*      $
��Bold"�5+      $��Center=�5,      ��5(      ��  �� Path$ HeadlineAlbum� J� Effect 5-      3  -System.Windows.Media.Effects.DropShadowEffect  5.         ShadowDepth$ 2Q�6'      	Direction$	 315Q�5/   !      Color$ Black��50   !   	   Opacity$		 1.0Q�51   !   
   
BlurRadius$
 1Q�52   !   54      55      �� 59   
   $��1��6   $
��Auto=�5:      $
��Auto=�5;      .��5=      �   PanelBackground#���  6    5>      �� 5?      $
��Auto �6$   �� 5@      $
��Auto �6$   �� 5A      $
��Auto �6$   .+�5D      �� $��0��6     5E        bandColumnVisibility 	 ProxyElement � DataContext��%X   &	       &		      �� 5F      67   � 5G       6@   �� $ SongsViewModel�5H      .+�5J      �� $y�	Collapsed=�6%   #��[	 6<   $   AllMyMusic.View.CustomDataGrid  5L      -   5          Name$ dataGridTracks��5L   (   $   AllMyMusic.View.CustomDataGrid   CellEditEnding$# dataGridTracks_CellEditEnding��5S      &   System.Windows.Controls.DataGrid   
IsReadOnly .  $	��200��5N      $��
10,10,10,0q�5O       5P      �� $ SongsViewModel�   SelectedItemsList 5Q      �� $ SelectedSongs� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=���5R      �� $ Songs�   Columns 5T      0  *System.Windows.Controls.DataGridTextColumn  5U      ,	  &System.Windows.Controls.DataGridColumn 	  Header$ Track��6K    	  
IsReadOnly .  1
  +System.Windows.Controls.DataGridBoundColumn 
  Binding 61   �� *Track+6o     5V      $ BandName��6N    .   61   �� *
BandName+ 	  
Visibility 5W   1   �� $# DataContext.BandColumnVisible� �� Source# [	  �� 	Converter# [ 5[        5\      $ 	SongTitle��6O    .   61   �� *	SongTitle+6w     5]      $ Duration��6N    .  61   �� *
Duration+6t   4  .System.Windows.Controls.DataGridTemplateColumn  5_      $ Rating��65      CellTemplate 5`   "   �� 5a   &   .R�5b   *   �� ��60   �� $ Rating�# [ 5c   '      CellEditingTemplate 5e   "   �� 5f   &   .R�5g   *   "   AllMyMusic.View.RatingSlider    Name$ ratingSlider1��6;    
 RatingSliderTemplate#��[
 5h   7   $��16��5i   7      ForegroundRectColor# �  5k   7      TagValue 5j   7   �� $ Rating�5k   7   5l   '   5n      5p      5q      �� 5t       �� Orientation$ 
Horizontal=�6   $��1��66   .+�5u      � $��10q�6     AlbumItemBackground#���  6'    5v      �� 5w      $
��Auto �6,   .+�5z      �� ���	#49FFFFFF$��2,2,0,0q�69   �� 5{      ���	#49000000$��0,0,2,2q�69   �� 5}        NoChromeButtonStyle#��[ 6L   ��6!   �� $ CoverImageCommand�.��5~      �� $��5q�5   %   $	��200��5�   %   $	��200��5�   %   ��5~   $   �� $ Album.FrontImageFullpath�# [  ��5�   %   �� $ SleeveImageSize���5�   %   �� $ SleeveImageSize�5�      5�      �� 5�      .+�5�      �� #��[ 6N   ��6!   �� $ CountryImageCommand�.��5�      �� $��30��5�   $   $��60��5�   $   $
��Left=�5�   $   $	��Top=�5�   $   $��10q�5�   $   ��5�   $   �� $ FlagPath�# [ ��5�   $   �� $# DataContext.CountryFlagHeight� �� RelativeSource �� *FindAncestor+  �� AncestorType   AllMyMusic.View.viewAlbum"    5�   $   �� $ Country�! � ToolTip! 5�   "   �� 5�   &   ��6,   �� $ Country�5�      5�      &    AllMyMusic.View.viewAlbumDetails  5�      $��	0,5,10,10q�5�       5�      �� $ AlbumDetails�5�      5�      5�      5�      "   AllMyMusic.View.viewCommands  5�   
   $��2��6   $��	20,5,20,5q�5�   5    6   �� 65   5�      5�      