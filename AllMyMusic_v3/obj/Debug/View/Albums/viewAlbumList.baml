   M S B A M L   `   `   `  ����E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View  5      #    AllMyMusic.View.viewAlbumList   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,vwclr-namespace:AllMyMusic.View     5        � 	Resources  5
       �� MergedDictionaries �� Source   pagerToVisibilityConverter1�� 5   
    5      �� 5      $. (../../ResourceDictionaries/MainSkin.xaml?�6%   �� 5      $0 *../../Datatemplates/AlbumItemTemplate.xaml?�6%   %   &	        �� 5      6+   5      .��5      �   PagerBackground#���  6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   ��   HeadlineBackground#���  6   $��0��6E   ._�5       ��   HeadlineText#���  5"      $��36(�5#      $
��Bold"�5$      $��Center=�5%      ��5!      ��  �� Path$ HeadlineAlbumList� J� Effect 5&      3  -System.Windows.Media.Effects.DropShadowEffect  5'         ShadowDepth$ 2Q�6'      	Direction$	 315Q�5(   !      Color$ Black��5)   !   	   Opacity$		 1.0Q�5*   !   
   
BlurRadius$
 1Q�5+   !   5-      5.      #   AllMyMusic.View.CustomListBox  50   
   -   5          Name$ 	albumList��50        PanelBackground#��[ 51      $��1��52      $��0q�53        AlbumItemTemplate#��[ 55      ��. $��Disabled=�5:      $
��Auto=�5;       %� IsVirtualizing .  %� VirtualizationMode$ 	Recycling=�5=      ��54      �� $ PageAlbumItemCollection� �� SelectedItem 56      �� $ SelectedAlbum� �� SelectionMode 57      �� $ SelectionMode� �� Mode$ TwoWay=�   SelectedAlbums 58      �� $! SelectedAlbumItemCollection�$ TwoWay=���5?      �� 5@      .R�5A      �  � Orientation$ 
Horizontal=�6    ��69   ��  �� ElementName$ 	albumList��$ ActualWidth�5B      ��5E      �� 5F       �� 
TargetType" ��6   .�5G      ��  �� Property �@��
IsSelected �� Value 63   �� *
IsSelected+$ TwoWay=�5H      5K         AllMyMusic.View.viewPager  5M   
   -   5          Name$ 
albumPager��5M      $��2��5N      $��	20,5,20,5q�5O       � DataContext 5P      �� $ Pager�y�5Q      �� $ PagerVisisble� �� 	Converter# [  5T      5W      