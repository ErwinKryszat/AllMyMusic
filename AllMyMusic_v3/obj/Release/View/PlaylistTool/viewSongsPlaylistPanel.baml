   M S B A M L   `   `   `  ���� E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View  ,    &AllMyMusic.View.viewSongsPlaylistPanel   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ,vwclr-namespace:AllMyMusic.View       � 	Resources   �� MergedDictionaries �� Source*  $AllMyMusic.View.RatingImageConverter   ratingImageConverter1.   (AllMyMusic.View.GridLengthValueConverter  gridLengthConverter1  boolToVisibilityConverter1��  �� $. (../../ResourceDictionaries/MainSkin.xaml?��� $6 0../../ResourceDictionaries/MainGridSplitter.xaml?��� $3 -../../ControlTemplates/GroupBoxTemplatey.xaml?��� $6 0../../ControlTemplates/RatingSliderTemplate.xaml?��� $# ../../Styles/ButtonStyle.xaml?��� $% ../../Styles/ComboBoxStyle.xaml?�%-   &	        &	      &	 
         �� .���   PagerBackground#���   � RowDefinitions �� $
��Auto ��� $��* �.+��� $��Insert a Field��$��0,0,0,0q�$��0��  GroupBoxTemplate#��[ .����  �� Orientation$ 
Horizontal=�$��Center=�.+�,   &AllMyMusic.View.viewQueryFieldSelector   � DataContext ��  �� Path$ QueryComposerViewModel��� $��Search��  ButtonStyle#��[ ���� $ SearchSongsCommand�� $��1�� � ColumnDefinitions �� ���� $! SongsLeft.SongListViewWidth� �� Mode$ TwoWay=�	 �� 	Converter#	 [ �� $
��Auto ��� $��60 ��� $��* ��� $
��Auto �.+�$   AllMyMusic.View.CustomDataGrid  $��0��$��	0,10,10,0q�&   System.Windows.Controls.DataGrid
   SelectionMode$
 Extended=� �� $ 	SongsLeft����� $ Songs�   SelectedItemsList �� $ SelectedSongs�$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Track��   
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Track+  $ BandName�� .  �� *
BandName+  $ 	AlbumName�� .  �� *	AlbumName+  $ 	SongTitle�� .  �� *	SongTitle+  $ Duration�� .  �� *
Duration+  $ Genre�� .  �� *Genre+  $ Country�� .  �� *	Country+  $ Language�� .  �� *
Language+  $ Comment�� .  �� *	Comment+  $
 Path�� .  �� *
SongPath+  $ Filename�� .  �� *SongFilename+4	  .System.Windows.Controls.DataGridTemplateColumn	  $ Rating�� 	  CellTemplate �� .R��� ���� $ Rating�#	 [   	  CellEditingTemplate �� .R�"
   AllMyMusic.View.RatingSlider
   
 Name$ ratingSlider1��  RatingSliderTemplate#��[ $��16��  PanelBackground 
  ForegroundRectColor# �   
  TagValue �� $ Rating��� $��1��$��10��$��Stretch=� �� ResizeDirection$ Columns=� �� ResizeBehavior$ PreviousAndCurrent=�  GridSplitterStyle1#���  �� $ Vertical=�$��2��$��0,60,0,0q�.+��� $��>>��#��[ ���� $ AddAllSongsCommand��� $��>��$��0,10,0,0q�#��[ ���� $ AddSelectedSongsCommand��� $��<��$��0,10,0,0q�#��[ ���� $  RemoveSelectedSongsCommand��� $��<<��$��0,10,0,0q�#��[ ���� $ RemoveAllSongsCommand�� $��3��$��	10,10,0,0q� �� $��* ��� $
��Auto �.+�  $��0��$
 Extended=� �� $ 
SongsRight����� $ Songs� �� $ SelectedSongs�$ TwoWay=�$ PropertyChanged=�   $ Track�� .  �� *Track+  $ BandName�� .  �� *
BandName+  $ 	AlbumName�� .  �� *	AlbumName+  $ 	SongTitle�� .  �� *	SongTitle+  $ Duration�� .  �� *
Duration+  $ Genre�� .  �� *Genre+  $ Country�� .  �� *	Country+  $ Language�� .  �� *
Language+  $ Comment�� .  �� *	Comment+  $
 Path�� .  �� *
SongPath+  $ Filename�� .  �� *SongFilename+	  $ Rating�� �� .R��� ���� $ Rating�#	 [   �� .R�
  $ ratingSlider1��#��[ $��16��# �   �� $ Rating�� $��1��$��4�� � Value �� $ ProgressValue�y��� $ ProgressBarVisible�#	 [ �� $ 
Horizontal=�$��4��$��0,60,0,0q�$��Center=�.+��� -   $��cmdMoveSongsUp��$��16��$��3,0,0,0q�#��[ ���� $ MoveSongsUpCommand����� $9��3/AllMyMusic;component/Resources/Images/Arrow_Up.png���� -   $��cmdMoveSongsDown��$��16��$��3,0,0,0q�#��[ ���� $ MoveSongsDownCommand����� $;��5/AllMyMusic;component/Resources/Images/Arrow_Down.png��