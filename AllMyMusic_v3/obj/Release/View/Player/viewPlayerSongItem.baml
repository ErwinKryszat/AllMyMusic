   M S B A M L   `   `   `  ���� E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View  (    "AllMyMusic.View.viewPlayerSongItem   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ,vwclr-namespace:AllMyMusic.View       � 	Resources   �� MergedDictionaries �� Source'   !AllMyMusic.View.PlaytimeConverter   playtimeConverter1  descriptionLabelStyle �� 
TargetType �� Property �� Value  bandLabelStyle  albumLabelStyle  songLabelStyle��  �� $. (../../ResourceDictionaries/MainSkin.xaml?��� $6 0../../ControlTemplates/RatingSliderTemplate.xaml?�%  &	        &	      &	 (     &	 c     &	 �       �� " ��.���  � ��$ 12(��� " ��.���  � ��$ 18(���  � ��$
 Bold"��� " ��.���  � ��$ 18(���  � ��$
 Bold"��� " ��.���  � ��$ 18(���  � ��$
 Bold"�.���   PagerBackground#���   � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto �.+�� $��0�� �� $
��Auto ��� $
��Auto ��� $
��Auto � � ColumnDefinitions �� $
��Auto ��� .+��� $��0��$��0��#��[ $��Right=�$��Center=�����  �� Path$ 	BandLabel��� $��1��$��0��#��[ $��Right=�$��Center=����� $ 
AlbumLabel��� $��2��$��0��#��[ $��Right=�$��Center=����� $ SongTitleLabel��� $��0��$��1��#��[ ���� $ Song.BandName��� $��1��$��1��#��[ ���� $ Song.AlbumName��� $��2��$��1��#��[ ���� $ Song.SongTitle��� 	 �� Orientation$	 
Horizontal=�$��1��.+��� #��[ ���� $ RatingLabel�"   AllMyMusic.View.RatingSlider  -   
  Name$
 ratingSlider��"   AllMyMusic.View.RatingSlider   PreviewMouseLeftButtonUp$+ %RatingSlider_PreviewMouseLeftButtonUp��  RatingSliderTemplate#��[ $��21��   ForegroundRectColor# �     TagValue �� $ Rating�!   AllMyMusic.View.viewCountry  $��10,0,0,0q�$��21��$��20��$��40�� � DataContext �� $ Country��� $	 
Horizontal=�$��2��.+��� $	 
Horizontal=�.+��� #��[ ���� $ CurrentTime��� $��/���� #��[ ���� $ Song.Duration��� $	 
Horizontal=�.+��� #��[ ���� $ 
TrackLabel��� $��0,5,5,5q�#��[ ���� $ 
Song.Track��� $	 
Horizontal=�.+��� #��[ ���� $ Song.Bitrate� � ToolTip �� $��Bitrate���� $
��kBps��$��0,5,5,5q�#��[ �� $	 
Horizontal=�$��10,0,0,0q�.+��� #��[ ���� $ Song.SampleRate� �� $��
Samplerate���� $	��kHz��$��0,5,5,5q�#��[ �� $��10,0,0,0q�#��[ ���� $ 
Song.Genre� �� ���� $ 
GenreLabel��� $	 
Horizontal=�$��10,0,0,0q�.+��� #��[ ���� $ 	YearLabel��� $��0,5,5,5q�#��[ ���� $ 	Song.Year� �� ���� $ 	YearLabel�