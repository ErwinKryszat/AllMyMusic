   M S B A M L   `   `   `  ����E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:AllMyMusic.ControlsAllMyMusic.Controls  5      %    AllMyMusic.View.viewToolbarTray   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9ammControls!clr-namespace:AllMyMusic.Controls     5        � 	Resources  5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $+ %../ResourceDictionaries/MainSkin.xaml?�6%   �� 5      $( "../Styles/AlphabetButtonStyle.xaml?�6%   �� 5      $, &../ControlTemplates/SearchTextBox.xaml?�6%   �� 5      $/ )../ControlTemplates/ExpanderTemplate.xaml?�6%   %    5      .��5      `� -   5         `�Name$ toolbarTray��5         HeadlineBackground `� 
Background# �   6'    `� IsLocked .  J� SnapsToDevicePixels . .�5   
   c� -   5         c�Name$ alphabetToolbar��5       c� Band$ 0��5      	 c� 	BandIndex$	 0��6   ��5      �� 
 �� Path$
 ButtonLabels���5      ��  �� ElementName$ toolbarTray��$
 
Background���5      �� 5      .R�5      d�   AlphabetButtonStyle#��[ 5   #   $��5,0,0,0q�5!   #   $��14(�64   $
��Bold"�6B   ��5   #   �� $
 	Character� d� 	IsChecked 5    #   �� $
 
IsSelected�5!   B   5"      5$      c� 5&   
   -   5        $ searchBandsToolbar��5&      $ 0��6.   $	 1��67   ��5'      �� $ toolbarTray��$
 
Background�.@�5*      '   !AllMyMusic.Controls.SearchTextBox  -   5          Name$ searchBandsTextBox��5*   (   $	��150��5+   (   $��25��5,   (      
SearchMode$ Delayed=�5-   (    � ToolTip$ Search Bands��5/   (      
SearchText 5.   (   �� $
 SearchBandsText� �� Mode$ TwoWay=�.��5/   ?   50      52      c� 54   
   -   5        $ 	vaToolbar��56      $ 1��54      $	 0��6   ��55      �� $ toolbarTray��$
 
Background�.@�59      d� -   5         d�Name$ ExpanderButton2��59        PagerBackground#��[ 5:      $��0��5;      $��0,5,0,0q�5<      $��Right=�5=      $��Center=�5>        AnimatedExpanderButtonWidth#��[ 5?       � OverridesDefaultStyle . $��1.5,0q�5B       5@      �� $
 AlbumListViewExpanded�5C      d� 5E      $��VA��6     ToolbarToggleButtonStyle#��[ 6(   $��15,0,5,0q�5F      $��14(�5H      $
��Bold"�6)    5G      �� $
 ViewVaGenres� 5I      �� 5J      ��6   �� $
 ViewVaGenres_ToolTip�5L      � 5N      .+�5O      �� $��View VA��6   $��5,0,15,0q�5Q      $��Center=�5R       5P      �� $
 ViewVaBands� 5S      �� 5T      ��6    �� $
 ViewVaBands_ToolTip�5V      5W      5Z      c� 5\   
   -   5        $ searchToolbar��5\      $ 1��6)   $	 1��62   ��5]      �� $ toolbarTray��$
 
Background�.@�5`        -   5        $ searchTextBox��5`   (   $	��200��5a   (   $��25��5b   (   $ Delayed=�5c   (   $ Search Songs��5e   (    5d   (   �� $
 
SearchText�$ TwoWay=�5e   (   5f      5h      5l      