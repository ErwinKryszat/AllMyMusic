   M S B A M L   `   `   `  ����H  DAllMyMusic_v3, Version=3.0.0.1, Culture=neutral, PublicKeyToken=null7 clr-namespace:AllMyMusic_v3.ViewAllMyMusic_v3.View  5      ?$clr-namespace:AllMyMusic_v3.ControlsAllMyMusic_v3.Controls  5      .    (AllMyMusic_v3.View.viewAlbumListAndAlbum   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /vw clr-namespace:AllMyMusic_v3.View     5      9controls$clr-namespace:AllMyMusic_v3.Controls     5      -   5          >�Name$  viewAlbumListAndAlbum1��5       � DataContext 5
      ��  � 	Resources 5       �� MergedDictionaries �� Source   albumVisibility�� 5   
    5      �� 5      $+ %../ResourceDictionaries/MainSkin.xaml?�6%   �� 5      $/ )../ControlTemplates/ExpanderTemplate.xaml?�6%   %   &	        �� 5      6+   5      .��5      � -   5         �Name$ 	albumGrid��5        PanelBackground#��[ 6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   9� $��0��6   $
��Left=�5        9� ExpandDirection$ Right=�5!        AdjustExpanderWidth#��[ 5"       � OverridesDefaultStyle . 	 9� 
IsExpanded	 5      �� 
 �� Path$'
 !DataContext.AlbumListViewExpanded� �� ElementName$ viewAlbumListAndAlbum1����5$      &    AllMyMusic_v3.View.viewAlbumList  5%       6$   �� $
 AlbumListViewModel�5(      "   AllMyMusic_v3.View.viewAlbum  5*   
   $��1��6    5+      �� $
 AlbumViewModel�y�5,      �� $"
 DataContext.AlbumViewVisible�$ viewAlbumListAndAlbum1�� �� 	Converter# [  52      5<      