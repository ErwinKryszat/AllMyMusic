   M S B A M L   `   `   `  ����E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View  5      +    %AllMyMusic.View.viewAlbumListAndAlbum   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,vwclr-namespace:AllMyMusic.View     5      -   5          >�Name$  viewAlbumListAndAlbum1��5       � DataContext 5	      ��  � 	Resources 5       �� MergedDictionaries �� Source   albumVisibility.   (AllMyMusic.View.GridLengthValueConverter  gridLengthConverter1�� 5   
    5      �� 5      $. (../../ResourceDictionaries/MainSkin.xaml?�6%   �� 5      $2 ,../../ControlTemplates/ExpanderTemplate.xaml?�6%   �� 5      $6 0../../ResourceDictionaries/MainGridSplitter.xaml?�6%   %:   &	        &	      �� 5      6+     5      6*   5      .��5      � -   5         �Name$ 	albumGrid��5        PanelBackground#��[ 6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   9� $��0��6   $
��Left=�5!       9� ExpandDirection$ Right=�5"        AdjustExpanderWidth#��[ 5#       � OverridesDefaultStyle . 	 9� 
IsExpanded	 5       �� 
 �� Path$'
 !DataContext.AlbumListViewExpanded� �� ElementName$ viewAlbumListAndAlbum1����5%      � 5&       5'      �� 5(      ��6+   �� $+
 %AlbumListViewModel.AlbumListViewWidth� �� Mode$ TwoWay=� �� 	Converter# [ �� 5)      $
��Auto �6+    � RowDefinitions 5+      �� 5,      $��* �6(   �� 5-      $
��Auto �6(   .+�50      #   AllMyMusic.View.viewAlbumList  $��0��6X   $��0��6i    6(   �� $
 AlbumListViewModel�6i   "   AllMyMusic.View.viewCommands  51      $��0��6&   $��1��66    52      �� $
 AlbumListViewModel��� 54      $��1��6#   $��2��55      $��10��56      $��Stretch=�57       �� ResizeDirection$ Columns=�58       �� ResizeBehavior$ PreviousAndCurrent=�59        GridSplitterStyle1#���  5:      5<      5@         AllMyMusic.View.viewAlbum  5B   
   $��1��6    5C      �� $
 AlbumViewModel�y�5D      �� $"
 DataContext.AlbumViewVisible�$ viewAlbumListAndAlbum1��# [  5G      5J      