   M S B A M L   `   `   `  ����E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View  5      ;"clr-namespace:AllMyMusic.ViewModelAllMyMusic.ViewModel  5      9!clr-namespace:AllMyMusic.ControlsAllMyMusic.Controls  5      *    $AllMyMusic.View.viewPropertiesEditor   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,vwclr-namespace:AllMyMusic.View     5      1vm"clr-namespace:AllMyMusic.ViewModel     5      9ammControls!clr-namespace:AllMyMusic.Controls     5        � 	Resources  5       �� MergedDictionaries �� Source   boolToVisibility-   'AllMyMusic.View.LongToFileSizeConverter  longToFilesizeConverter1(   "AllMyMusic.View.FrequencyFormatter  frequencyFormatter1  headlineStyle �� 
TargetType �� Property �� Value  formTextBoxStyle%   AllMyMusic.Controls.FormTextBox  formLabelStyle#   AllMyMusic.Controls.FormLabel  formLabelUOMStyle&    AllMyMusic.Controls.FormLabelUOM  formComboBoxStyle&    AllMyMusic.Controls.FormComboBox  formRatingStyle$   AllMyMusic.Controls.FormRating�� 5   
    5      �� 5      $. (../../ResourceDictionaries/MainSkin.xaml?�6%   �� 5      $6 0../../ResourceDictionaries/MainGridSplitter.xaml?�6%   %P  &	        &	      &	 &     &	 9     &	 �     &	 �    &	 �    &	 �    &	 �    �� 5      6+     5      6*     5      6$   �� 5      " ��6*   .�5      ��  � ��$ Center=�66   �� 5       � ��$ 14(�6-   5      �� 5      "  6-   .�5      ��  � ��$ 0,3,0,0q�6+   �� 5       �@ 
LabelWidth$	 120Q�6/   �� 5        �@ LabelPadding$ 15,0,0,0q�61   �� 5!      ' �@ LabelHorizontalContentAlignment$
 Left=�6D   �� 5"       �@ TextBoxMargin$ 0,0,15,0q�62   5#      �� 5%      "  6+   .�5&      ��  � ��$ 0,3,0,0q�6+   �� 5'       �@ 
LabelWidth$	 120Q�6/   �� 5(       �@ LabelPadding$ 15,0,0,0q�61   �� 5)      ' �@ LabelHorizontalContentAlignment$
 Left=�6D   5*      �� 5,      "  6.   .�5-      ��  � ��$ 0,3,0,0q�6+   �� 5.       �@ 
LabelWidth$	 120Q�6/   �� 5/       �@ LabelPadding$ 15,0,0,0q�61   �� 50      ' �@ LabelHorizontalContentAlignment$
 Left=�6D   51      �� 53      "  6.   .�54      ��  � ��$ 0,3,0,0q�6+   �� 55       �@ 
LabelWidth$	 120Q�6/   �� 56       �@ LabelPadding$ 15,0,0,0q�61   �� 57      ' �@ LabelHorizontalContentAlignment$
 Left=�6D   �� 58       �@ ComboBoxWidth$	 100Q�62   59      �� 5;      "  6,   .�5<      ��  � ��$ 0,3,0,0q�6+   �� 5=       �@ 
LabelWidth$	 120Q�6/   �� 5>       �@ LabelPadding$ 15,0,0,0q�61   �� 5?      ' �@ LabelHorizontalContentAlignment$
 Left=�6D   5@      5B      .��5H      �� $
��Auto=�6   $
��Auto=�5I      .��5K   
   ��  �� Orientation$ Vertical=�6   
�������.+�5L      �� -   5        $��lableContributors��5L      #��[ 61   $��0,10,0,0q�6�   $
��Bold"�6�   ��6X   ��  �� Path$ ContributorsLabel�6�     5N      -   5          Name$ leadPerformer��5N   *   #��[ 6�   	   Label	 6A   �� $ LeadPerformerLabel�
   Text
 6k   �� $ Song.LeadPerformer� �� Mode$ TwoWay=�6�     5O      -   5        $ bandName��5O   *   #��[ 6�   	 6<   �� $ 	BandLabel�
 6]   �� $ Song.BandName�$ TwoWay=�6�     5P      -   5        $ bandSortName��5P   *   %   AllMyMusic.Controls.FormTextBox!   PreviewMouseLeftButtonDown$- 'bandSortName_PreviewMouseLeftButtonDown��6�   #��[ 6�   	 6@   �� $ BandSortLabel�
 6e   �� $ Song.BandSortName�$ TwoWay=�6�     5Q      -   5        $ composerName��5Q   *   #��[ 6�   	 6@   �� $ ComposerLabel�
 6e   �� $ Song.ComposerName�$ TwoWay=�6�     5R      -   5        $ conductorName��5R   *   #��[ 6�   	 6A   �� $ ConductorLabel�
 6g   �� $ Song.ConductorName�$ TwoWay=�6�   �� 5T      -   5        $��lableAlbumInformation��5T      #��[ 65   $��0,10,0,0q�6�   $
��Bold"�6�   ��6\   �� $ Album_InfoLabel�6�     5V      -   5        $ album��5V   *   #��[ 6�   	 69   �� $ 
AlbumLabel�
 6[   �� $ Song.AlbumName�$ TwoWay=�6�     5W      -	   5          Name$
 path��5W   (   #��[ 6{      Label 66   �� $ 	PathLabel�   Text 6W   �� $ Song.SongPath�6{   �� 5Y      $ 
Horizontal=�6   .+�5Z        -
   5          Name$ 
cbLanguage��5Z   /   #��[ 6�      Label 6C   �� $ LanguageLabel�   ItemList 6h   �� $ 	Languages�   SelectedItem 6�   �� $ Song.Language�$ TwoWay=�6�     5[      -   5        $ 	cbCountry��5[   /   #��[ 6�    6B   �� $ CountryLabel� 6f   �� $ 	Countries� 6�   �� $ Song.Country�$ TwoWay=�6�   !	   AllMyMusic.View.viewCountry	  5\      $��20,0,0,0q�5]      $��21��5^       � DataContext 5_      �� $ Country�5a      �� 5c      $ 
Horizontal=�6   .+�5d        -   5        $ 
albumGenre��5d   ,   #��[ 6�      ContentWidth$	 100Q�6�    6@   �� $ AlbumGenreLabel� 6g   �� $ Song.AlbumGenre�6�     5e      -   5        $
 year��5e   .   #��[ 6�      TextBoxWidth$ 50Q�6�   	 6<   �� $ 	YearLabel�
 6]   �� $ 	Song.Year�$ TwoWay=�6�   5f        5g      -   5        $ variousArtists��5g   (   #��[ 6�    6@   �� $ ArtistTypeLabel� 6g   �� $ Song.ArtistType�6�   �� 5j      -   5        $��lableSongInformation��5j      #��[ 64   $��0,10,0,0q�6�   $
��Bold"�6�   ��6[   �� $ Song_InfoLabel�6�     5l      -   5        $ 	songTitle��5l   *   #��[ 6�   	 6=   �� $ SongTitleLabel�
 6c   �� $ Song.SongTitle�$ TwoWay=�y�5m   .   �� $ 
SingleSong� �� 	Converter# [    5n      -   5        $ filename��5n   (   #��[ 6�    6:   �� $ FilenameLabel� 6_   �� $ Song.SongFilename�y�5o   -   �� $ 
SingleSong�# [  �� 5q      $ 
Horizontal=�6   
�������.+�5r        -   5        $ cbGenre��5r   /   #��[ 6�    6@   �� $ 
GenreLabel� 6b   �� $ Genres� 6�   �� $ 
Song.Genre�$ TwoWay=�6�     5s      -   5          Name$ ratingSlider��5s   -   #��[ 6�      Label 6C   �� $ RatingLabel�   Rating 6f   �� $ Song.Rating�$ TwoWay=�6�   5t      �� 5v      $ 
Horizontal=�6   .+�5w        -   5        $ length��5w   ,   #��[ 6�   $	 100Q�6�    6<   �� $ LengthLabel� 6_   �� $ Song.Duration�y�5x   0   �� $ 
SingleSong�# [    5y      -   5        $ track��5y   .   #��[ 6�   $ 50Q�6�   	 6=   �� $ 
TrackLabel�
 6_   �� $ 
Song.Track�$ TwoWay=�y�5z   0   �� $ 
SingleSong�# [  5{        5}      -   5        $ songComment��5}   *   #��[ 6�   $��60��6�   	 6?   �� $ CommentLabel�
 6c   �� $ Song.Comment�$ TwoWay=�6�   �� 5�      -   5        $��lableTechnicalInformation��5�      #��[ 69   $
��Auto��6�   $��0,10,0,0q�6�   $
��Bold"�6�   ��6`   �� $ Technical_InfoLabel�6�     5�      -   5          Name$ samplingRate��5�   +   
   UOM$	 kHz��6�   #��[ 6�      Label 6A   �� $ SampleRateLabel�   Text 6h   �� $ Song.SampleRate�# [  �� ConverterParameter$
 ##.#��6�     5�      -   5        $ bitrate��5�   +   $
 kBps��6�   #��[ 6�    6<   �� $ BitrateLabel� 6`   �� $ Song.Bitrate�# [ $	 ###��6�     5�      -   5        $ bitrateType��5�   (   #��[ 6�    6=   �� $ BitrateTypeLabel� 6e   �� $ Song.BitrateType�  � ToolTip  5�      �� 5�      $ Vertical=�6%   .+�5�      �� .��6$   CBR = Constant Bitrate6<   �� 5�      .��6$   VBR = Variable Bitrate6<   5�      5�        5�      -   5        $ channels��5�   (   #��[ 6�   $	 100Q�6�    6:   �� $ ChannelsLabel� 6_   �� $ Song.ChannelMode�y�5�   ,   �� $ 
SingleSong�# [    5�      -   5        $ filesize��5�   (   #��[ 6�   $	 100Q�6�    6:   �� $ FileSizeLabel� 6_   �� $ Song.FileSize�# [ y�5�   ,   �� $ 
SingleSong�# [  �� 5�      -   5        $��lableWebsiteInformation��5�      #��[ 67   $
��Auto��6�   $��0,10,0,0q�6�   $
��Bold"�6�   ��6^   �� $ Website_InfoLabel�6�     5�      -   5        $ websiteUser��5�   *   #��[ 6�   	 6?   �� $ Website_UserLabel�
 6h   �� $ Song.WebsiteUser�$ TwoWay=�6�     5�      -   5        $ websiteArtist��5�   *   #��[ 6�   	 6A   �� $ Website_ArtistLabel�
 6l   �� $ Song.WebsiteArtist�$ TwoWay=�6�   5�      5�      5�      