   M S B A M L   `   `   `  ����H  DAllMyMusic_v3, Version=3.0.0.1, Culture=neutral, PublicKeyToken=null7 clr-namespace:AllMyMusic_v3.ViewAllMyMusic_v3.View  5      %    AllMyMusic_v3.View.viewPlayList   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Hei9http://schemas.microsoft.com/expression/2010/interactions     5      /vw clr-namespace:AllMyMusic_v3.View     5        � 	Resources  5       �� MergedDictionaries �� Source   pagerToVisibilityConverter1�� 5   
    5      �� 5      $+ %../ResourceDictionaries/MainSkin.xaml?�6%   �� 5      $, &../Datatemplates/SongItemTemplate.xaml?�6%   %   &	        �� 5      6+   5      .��5      �   PagerBackground#���  5       � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5!   
   �� -   5         ��Name$ headlineBroder��5!      $��0��6*     HeadlineBackground#���  67   ._�5"      ��   HeadlineText#���  5#      $��36(�5$      $
��Bold"�5%      $��Center=�5&      ��5"      ��  �� Path$ Headline� J� Effect 5'      3  -System.Windows.Media.Effects.DropShadowEffect  5(         ShadowDepth$ 2Q�6'      	Direction$	 315Q�5)      	   Color$	 Black��5*      
   Opacity$	
 1.0Q�5+         
BlurRadius$ 1Q�5,      5.      5/      )   #AllMyMusic_v3.View.ListBoxSongItems  51   
   -   5          Name$ playList��51        PanelBackground#��[ 52      $��1��54      $��0q�55        SongItemTemplate#��[ 57      ��. $��Disabled=�5:      $
��Auto=�5;       %� IsVirtualizing .  %� VirtualizationMode$ 	Recycling=�5=       �� SelectionMode$ Multiple=�5?      ��53      ��  �� ElementName$ headlineBroder��$ ActualWidth���56      �� $ PageSongItemCollection�   InsertSongsIndex 5>      �� $ InsertSongsIndex� �� Mode$ OneWayToSource=���5A      �� 5B       �� 
TargetType" ��6   .�5C      ��  �� Property � �� �� Value$ Stretch=�6C   �� 5D       �@��
IsSelected 63   �� *
IsSelected+$ TwoWay=�5E      5H      "   AllMyMusic_v3.View.viewPager  5J   
   -   5          Name$ 
songsPager��5J      $��3��5K      $��	20,5,20,5q�5L       � DataContext 5M      �� $ Pager�y�5N      �� $ PagerVisisble� �� 	Converter# [  5O      5S      