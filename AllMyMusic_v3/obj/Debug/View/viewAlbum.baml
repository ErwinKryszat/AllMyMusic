   M S B A M L   `   `   `  ����H  DAllMyMusic_v3, Version=3.0.0.1, Culture=neutral, PublicKeyToken=null7 clr-namespace:AllMyMusic_v3.ViewAllMyMusic_v3.View  5      "    AllMyMusic_v3.View.viewAlbum   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /vw clr-namespace:AllMyMusic_v3.View     5      -   5        $��
viewAlbum1��5        � 	Resources  5       �� MergedDictionaries �� Source'   !AllMyMusic_v3.View.ImageConverter   imagePathToImageConverter  headlineVisibility�� 5   
    5      �� 5      $+ %../ResourceDictionaries/MainSkin.xaml?�6%   �� 5      $( "../Styles/NoChromeButtonStyle.xaml?�6%   %:   &	        &	        5      6    �� 5      6+   5      .��5      �   PagerBackground#���  6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5"   
   ��   HeadlineBackground#���  6   $��0��6E   ._�5#      ��   HeadlineText#���  5%      $��36(�5&      $
��Bold"�5'      $��Center=�5(      ��5$      ��  �� Path$ HeadlineAlbum� J� Effect 5)      3  -System.Windows.Media.Effects.DropShadowEffect  5*         ShadowDepth$ 2Q�6'      	Direction$	 315Q�5+   !      Color$ Black��5,   !   	   Opacity$		 1.0Q�5-   !   
   
BlurRadius$
 1Q�5.   !   50      51      �� 55   
   $��1��6   $
��Auto=�56      $
��Auto=�57      .��59      �   PanelBackground#���  6    5:      �� 5;      $
��Auto �6$   �� 5<      $
��Auto �6$   �� 5=      $
��Auto �6$   .+�5@      '   !AllMyMusic_v3.View.CustomDataGrid  -   5          Name$ dataGridTracks��5@   $   $��0��5A      $	��200��5B      $��
10,10,10,0q�5C       � DataContext 5D      �� $ SongsViewModel�   SelectedItemsList 5E      �� $ SelectedSongs� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=���5F      �� $ Songs��� 5H       �� Orientation$ 
Horizontal=�6   $��1��66   .+�5I      � $��10q�6     AlbumItemBackground#���  6'    5J      �� 5K      $
��Auto �6,   .+�5N      �� ���	#49FFFFFF$��2,2,0,0q�69   �� 5O      ���	#49000000$��0,0,2,2q�69   �� 5Q        NoChromeButtonStyle#��[ 6L   ��6!   �� $ CoverImageCommand�.��5R      �� $��5q�5S   %   $	��200��5T   %   $	��200��5U   %   ��5R   $   �� $ Album.FrontImageFullpath� �� 	Converter# [  ��5V   %   �� $ SleeveImageSize���5W   %   �� $ SleeveImageSize�5X      5Y      �� 5[      .+�5\      $   AllMyMusic_v3.View.viewCountry  $��30��5^      $��60��5_      $
��Left=�5`      $	��Top=�5a      $��10q�5b      ��5]      �� $# DataContext.CountryFlagHeight� �� RelativeSource �� *FindAncestor+ �� AncestorType"   AllMyMusic_v3.View.viewAlbum"   5c      �� $ Country�)   #AllMyMusic_v3.View.viewAlbumDetails  5f      $��	0,5,10,10q�5g       5h      �� $ AlbumDetails�5j      5k      5l      5m      %   AllMyMusic_v3.View.viewCommands  5o   
   $��2��6   $��	20,5,20,5q�5p   5    6   �� 65   5s      5v      