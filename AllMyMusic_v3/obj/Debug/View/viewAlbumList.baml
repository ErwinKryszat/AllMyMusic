   M S B A M L   `   `   `  ����H  DAllMyMusic_v3, Version=3.0.0.1, Culture=neutral, PublicKeyToken=null7 clr-namespace:AllMyMusic_v3.ViewAllMyMusic_v3.View  5      &     AllMyMusic_v3.View.viewAlbumList   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /vw clr-namespace:AllMyMusic_v3.View     5        � 	Resources  5
       �� MergedDictionaries �� Source   pagerToVisibilityConverter11   +AllMyMusic_v3.View.GridLengthValueConverter  gridLengthConverter1�� 5   
    5      �� 5      $+ %../ResourceDictionaries/MainSkin.xaml?�6%   �� 5      $- '../Datatemplates/AlbumItemTemplate.xaml?�6%   �� 5      $3 -../ResourceDictionaries/MainGridSplitter.xaml?�6%   %:   &	        &	      �� 5      6+     5      6*   5      .��5      �   PagerBackground#���  6    � ColumnDefinitions 5   
   �� 5      ��6   ��  �� Path$ AlbumListViewWidth� �� Mode$ TwoWay=� �� 	Converter# [ �� 5      $
��Auto �6    � RowDefinitions 5    
   �� 5!      $
��Auto �6   �� 5"      $��* �6   �� 5#      $
��Auto �6   �� 5$      $
��Auto �6   .+�5'   
   ��   HeadlineBackground#���  6   $��0��6E   ._�5(      ��   HeadlineText#���  5*      $��36(�5+      $
��Bold"�5,      $��Center=�5-      ��5)      �� $ HeadlineAlbumList� J� Effect 5.      3  -System.Windows.Media.Effects.DropShadowEffect  5/      	   ShadowDepth$	 2Q�6'   
   	Direction$	
 315Q�50   !      Color$ Black��51   !      Opacity$	 1.0Q�52   !      
BlurRadius$ 1Q�53   !   55      56      �� 58   
   -   5         ��Name$ 	albumList��58        PanelBackground#��[ 59      $��1��5:      $��0��5;      $��0q�5<        AlbumItemTemplate#��[ 5>      ��. $��Disabled=�5A      $
��Auto=�5B       %� IsVirtualizing .  %� VirtualizationMode$ 	Recycling=�5D      ��5=      �� $ PageAlbumItemCollection� �� SelectedItem 5?      �� $ SelectedAlbum���5F      �� 5G      .R�5H      �  � Orientation$ 
Horizontal=�6    ��69   ��  �� ElementName$ 	albumList��$ ActualWidth�5I      5K      %   AllMyMusic_v3.View.viewCommands  5N   
   $��2��6   $��0��5O      $��	20,5,20,5q�5P       � DataContext 5Q      �� "   AllMyMusic_v3.View.viewPager  5S   
   -   5          Name$ 
albumPager��5S      $��3��5T      $��0��5U      $��	20,5,20,5q�5V       5W      �� $ Pager�y�5X      �� $ PagerVisisble�# [  �� 5Z   
   $��1��6   $��4��5[      $��10��5\      $��Stretch=�5]       �� ResizeDirection$ Columns=�5^       �� ResizeBehavior$ PreviousAndCurrent=�5_        GridSplitterStyle1#���  5`      5b      5c      5k      