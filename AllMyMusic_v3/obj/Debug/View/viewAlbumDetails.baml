   M S B A M L   `   `   `  ����H  DAllMyMusic_v3, Version=3.0.0.1, Culture=neutral, PublicKeyToken=null7 clr-namespace:AllMyMusic_v3.ViewAllMyMusic_v3.View  5      )    #AllMyMusic_v3.View.viewAlbumDetails   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /vw clr-namespace:AllMyMusic_v3.View     5        � 	Resources  5      *   $AllMyMusic_v3.View.PlaytimeConverter   playtimeConverter1(   "AllMyMusic_v3.View.NumberFormatter  numberFormatter1?   9AllMyMusic_v3.View.StringNullOrEmptyToVisibilityConverter + 'stringNullOrEmptyToVisibilityConverter1��%W   &	        &	      &	 &       5   
   6     5   
   6     5   
   64   5      .��5      �� .+�5   
   ��  �� Orientation$ 
Horizontal=�6   .+�5      �� .��6   Year:6   �� 5      ��6   ��  �� Path$
 Year�64   5      �� 5   
   $ 
Horizontal=�6   .+�5      �� .��6   Genre:6   �� 5      ��6   �� $ 
AlbumGenre�6:   5      �� 5   
   $ 
Horizontal=�6   .+�5      �� .��6   Total Length:6#   �� 5      ��6   �� $ TotalLength� �� 	Converter# [  6j   5      �� 5   
   $ 
Horizontal=�6   .+�5      �� .��6   Sampling Rate:6$   �� 5       .��5!      �� ��6   �� $ SamplingRate�# [  �� ConverterParameter$
 ##.#��5"      �� 5#      .��6   kHz6   5$      �� 5%   
   $ 
Horizontal=�6   .+�5&      �� .��6   	Bitrate6   �� 5'      ��6   �� $ Bitrate�67   �� 5(      .��6   kBps6   5)      �� 5+   
   $��5q�6   y�5,      �� $ WebsiteUser�# [ �� 5-      -   5        ��5-      �� $ WebsiteUser�.�6m   Website User5/       � ToolTip 50      �� 51      .��6   User defined Website6.   53      �� 56   
   $��5q�6   y�57      �� $ WebsiteArtist�# [ �� 58      -   5        ��58      �� $ WebsiteArtist�.�6o   Website Artist5:       5;      �� 5<      .��6   Official Artist Website61   5>      5@      5B      