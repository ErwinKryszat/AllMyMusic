   M S B A M L   `   `   `  ����E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View  5      (    "AllMyMusic.View.viewPlayerSongItem   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,vwclr-namespace:AllMyMusic.View     5        � 	Resources  5       �� MergedDictionaries �� Source'   !AllMyMusic.View.PlaytimeConverter   playtimeConverter1  descriptionLabelStyle �� 
TargetType �� Property �� Value  bandLabelStyle  albumLabelStyle  songLabelStyle�� 5   
    5      �� 5      $. (../../ResourceDictionaries/MainSkin.xaml?�6%   �� 5      $6 0../../ControlTemplates/RatingSliderTemplate.xaml?�6%   %�  &	        &	      &	 [     &	 �     &	 7      5      6#   �� 5      " ��62   .�5      ��  � ��$ 12(�6-   5      �� 5      " ��6+   .�5      ��  � ��$ 18(�6-   �� 5       � ��$
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
��Auto �6#   �� 5=      .+�5@      �� $��0��6   $��0��6!   #��[ 6T   $��Right=�6�   $��Center=�6�   ��61   ��  �� Path$ 	BandLabel�6�   �� 5A      $��1��6   $��0��6!   #��[ 6U   $��Right=�6�   $��Center=�6�   ��61   �� $ 
AlbumLabel�6�   �� 5B      $��2��6   $��0��6!   #��[ 6Y   $��Right=�6�   $��Center=�6�   ��61   �� $ SongTitleLabel�6�   �� 5D      $��0��6   $��1��6!   #��[ 6X   ��61   �� $ Song.BandName�6X   �� 5E      $��1��6   $��1��6!   #��[ 6Y   ��61   �� $ Song.AlbumName�6Y   �� 5F      $��2��6   $��1��6!   #��[ 6Y   ��61   �� $ Song.SongTitle�6Y   5G      �� 5J   
   	 �� Orientation$	 
Horizontal=�6   $��1��6.   .+�5L      �� #��[ 69   ��6   �� $ RatingLabel�69   "   AllMyMusic.View.RatingSlider  5N      -   5        
  Name$
 ratingSlider��5O      "   AllMyMusic.View.RatingSlider   PreviewMouseLeftButtonUp$+ %RatingSlider_PreviewMouseLeftButtonUp��5T        RatingSliderTemplate#��[ 5P      $��21��5Q         ForegroundRectColor# �  5S         TagValue 5R      �� $ Rating�5T      !   AllMyMusic.View.viewCountry  5W      $��10,0,0,0q�5X      $��21��5Y      $��20��5Z      $��40��5[       � DataContext 5\      �� $ Country�5_      �� 5d   
   $	 
Horizontal=�6   $��2��6/   .+�5f      �� $	 
Horizontal=�6   .+�5g      �� #��[ 6=   ��6   �� $ CurrentTime�6=   �� 5h      $��/��6   �� 5i      #��[ 6?   ��6   �� $ Song.Duration�6?   5j      �� 5l      $	 
Horizontal=�6   .+�5m      �� #��[ 6<   ��6   �� $ 
TrackLabel�6<   �� 5n      $��0,5,5,5q�6=   #��[ 6O   ��6   �� $ 
Song.Track�6O   5o      �� 5q      $	 
Horizontal=�6   .+�5r      �� #��[ 6>   ��6   �� $ Song.Bitrate� � ToolTip 5s      �� 5t      $��Bitrate��6    5v      �� 5w      $
��kBps��6   $��0,5,5,5q�6'   #��[ 69   5x      �� 5z      $	 
Horizontal=�6   $��10,0,0,0q�62   .+�5{      �� #��[ 6C   ��6   �� $ Song.SampleRate� 5|      �� 5}      $��
Samplerate��6    5      �� 5�      $	��kHz��6   $��0,5,5,5q�6'   #��[ 69   5�      �� 5�      $��10,0,0,0q�6   #��[ 6J   ��6&   �� $ 
Song.Genre� 5�      �� 5�      ��6   �� $ 
GenreLabel�5�      �� 5�      $	 
Horizontal=�6   $��10,0,0,0q�62   .+�5�      �� #��[ 6<   ��6   �� $ 	YearLabel�6<   �� 5�      $��0,5,5,5q�6;   #��[ 6M   ��6   �� $ 	Song.Year� 5�      �� 5�      ��6    �� $ 	YearLabel�5�      5�      5�      5�      5�      