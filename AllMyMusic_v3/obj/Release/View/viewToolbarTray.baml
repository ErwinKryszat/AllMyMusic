   M S B A M L   `   `   `  ���� E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:AllMyMusic.ControlsAllMyMusic.Controls  %    AllMyMusic.View.viewToolbarTray   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     9ammControls!clr-namespace:AllMyMusic.Controls       � 	Resources   �� MergedDictionaries �� Source��  �� $+ %../ResourceDictionaries/MainSkin.xaml?��� $( "../Styles/AlphabetButtonStyle.xaml?��� $, &../ControlTemplates/SearchTextBox.xaml?��� $/ )../ControlTemplates/ExpanderTemplate.xaml?�%    .��`� -    `�Name$ toolbarTray��   HeadlineBackground `� 
Background# �    `� IsLocked .  J� SnapsToDevicePixels . .�c� -    c�Name$ alphabetToolbar�� c� Band$ 0��	 c� 	BandIndex$	 0������ 
 �� Path$
 ButtonLabels�����  �� ElementName$ toolbarTray��$
 
Background����� .R�d�   AlphabetButtonStyle#��[ $��5,0,0,0q�$��14(�$
��Bold"����� $
 	Character� d� 	IsChecked �� $
 
IsSelected�c� -   $ searchBandsToolbar��$ 0��$	 1������ $ toolbarTray��$
 
Background�.@�'   !AllMyMusic.Controls.SearchTextBox  -     Name$ searchBandsTextBox��$	��150��$��25��   
SearchMode$ Delayed=� � ToolTip$ Search Bands��   
SearchText �� $
 SearchBandsText� �� Mode$ TwoWay=�.��c� -   $ 	vaToolbar��$ 1��$	 0������ $ toolbarTray��$
 
Background�.@�d� -    d�Name$ ExpanderButton2��  PagerBackground#��[ $��0��$��0,5,0,0q�$��Right=�$��Center=�  AnimatedExpanderButtonWidth#��[  � OverridesDefaultStyle . $��1.5,0q� �� $
 AlbumListViewExpanded�d� $��VA��  ToolbarToggleButtonStyle#��[ $��15,0,5,0q�$��14(�$
��Bold"� �� $
 ViewVaGenres� �� ���� $
 ViewVaGenres_ToolTip�� .+��� $��View VA��$��5,0,15,0q�$��Center=� �� $
 ViewVaBands� �� ���� $
 ViewVaBands_ToolTip�c� -   $ searchToolbar��$ 1��$	 1������ $ toolbarTray��$
 
Background�.@�  -   $ searchTextBox��$	��200��$��25��$ Delayed=�$ Search Songs�� �� $
 
SearchText�$ TwoWay=�