   M S B A M L   `   `   `  ����H  DAllMyMusic_v3, Version=3.0.0.1, Culture=neutral, PublicKeyToken=null7 clr-namespace:AllMyMusic_v3.ViewAllMyMusic_v3.View  5      +    %AllMyMusic_v3.View.viewPlayerSongItem   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /vw clr-namespace:AllMyMusic_v3.View     5        � 	Resources  5       �� MergedDictionaries �� Source*   $AllMyMusic_v3.View.PlaytimeConverter   playtimeConverter1  descriptionLabelStyle �� 
TargetType �� Property �� Value  bandLabelStyle  albumLabelStyle  songLabelStyle�� 5   
    5      �� 5      $+ %../ResourceDictionaries/MainSkin.xaml?�6%   �� 5      $3 -../ControlTemplates/RatingSliderTemplate.xaml?�6%   %�  &	        &	      &	 [     &	 �     &	 7      5      6#   �� 5      " ��62   .�5      ��  � ��$ 12(�6-   5      �� 5      " ��6+   .�5      ��  � ��$ 18(�6-   �� 5       � ��$
 Bold"�6/   5      �� 5      " ��6,   .�5      ��  � ��$ 18(�6-   �� 5       � ��$
 Bold"�6/   5       �� 5"      " ��6+   .�5#      ��  � ��$ 18(�6-   �� 5$       � ��$
 Bold"�6/   5%      5'      .��5*      �   PagerBackground#���  6    � RowDefinitions 5-   
   �� 5.      $
��Auto �6   �� 5/      $
��Auto �6   �� 50      $
��Auto �6   .+�54   
   � $��0��6    55      �� 56      $
��Auto �6    �� 57      $
��Auto �6    �� 58      $
��Auto �6     � ColumnDefinitions 5;      �� 5<      $
��Auto �6#   �� 5=      .+�5@      �� $��0��6   $��0��6!   $
��Band��61   #��[ 6@   $��Right=�6o   $��Center=�6�   �� 5A      $��1��6   $��0��6!   $��Album��61   #��[ 6A   $��Right=�6p   $��Center=�6�   �� 5B      $��2��6   $��0��6!   $
��Song��61   #��[ 6@   $��Right=�6o   $��Center=�6�   �� 5D      $��0��6   $��1��6!   #��[ 6X   ��61   ��  �� Path$ Song.BandName�6X   �� 5E      $��1��6   $��1��6!   #��[ 6Y   ��61   �� $ Song.AlbumName�6Y   �� 5F      $��2��6   $��1��6!   #��[ 6Y   ��61   �� $ Song.SongTitle�6Y   5G      �� 5J   
   	 �� Orientation$	 
Horizontal=�6   $��1��6.   .+�5L      �� $��Rating��6   #��[ 6%   %   AllMyMusic_v3.View.RatingSlider  5N        RatingSliderTemplate#��[ 6   $��21��5O      
   ForegroundRectColor#
 �  5Q         TagValue 5P      �� $ Song.Rating�5Q      $   AllMyMusic_v3.View.viewCountry  5S      $��10,0,0,0q�5T      $��21��5U      $��20��5V      $��40��5W       � DataContext 5X      �� $ Country�5[      �� 5`   
   $	 
Horizontal=�6   $��2��6/   .+�5b      �� $	 
Horizontal=�6   .+�5c      �� #��[ 6=   ��6   �� $ CurrentTime�6=   �� 5d      $��/��6   �� 5e      #��[ 6?   ��6   �� $ Song.Duration�6?   5f      �� 5h      $	 
Horizontal=�6   .+�5i      �� $��Track��6   #��[ 6(   �� 5j      $��0,5,5,5q�6=   #��[ 6O   ��6   �� $ 
Song.Track�6O   5k      �� 5m      $	 
Horizontal=�6   .+�5n      �� #��[ 6>   ��6   �� $ Song.Bitrate� � ToolTip 5o      �� 5p      $��Bitrate��6    5r      �� 5s      $
��kBps��6   $��0,5,5,5q�6'   #��[ 69   5t      �� 5v      $	 
Horizontal=�6   $��10,0,0,0q�62   .+�5w      �� #��[ 6C   ��6   �� $ Song.SampleRate� 5x      �� 5y      $��
Samplerate��6    5{      �� 5|      $	��kHz��6   $��0,5,5,5q�6'   #��[ 69   5}      �� 5      $��10,0,0,0q�6   #��[ 6J   ��6&   �� $ 
Song.Genre� 5�      �� 5�      $��Genre��6   5�      �� 5�      $	 
Horizontal=�6   $��10,0,0,0q�62   .+�5�      �� $
��Year��6   #��[ 6(   �� 5�      $��0,5,5,5q�6;   #��[ 6M   ��6   �� $ 	Song.Year� 5�      �� 5�      $
��Year��6    5�      5�      5�      5�      5�      