   M S B A M L   `   `   `  ����H  DAllMyMusic_v3, Version=3.0.0.1, Culture=neutral, PublicKeyToken=null7 clr-namespace:AllMyMusic_v3.ViewAllMyMusic_v3.View  5      A%clr-namespace:AllMyMusic_v3.ViewModelAllMyMusic_v3.ViewModel  5      ?$clr-namespace:AllMyMusic_v3.ControlsAllMyMusic_v3.Controls  5      -    'AllMyMusic_v3.View.viewPropertiesEditor   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /vw clr-namespace:AllMyMusic_v3.View     5      4vm%clr-namespace:AllMyMusic_v3.ViewModel     5      <ammControls$clr-namespace:AllMyMusic_v3.Controls     5        � 	Resources  5       �� MergedDictionaries �� Source   boolToVisibility0   *AllMyMusic_v3.View.LongToFileSizeConverter  longToFilesizeConverter1+   %AllMyMusic_v3.View.FrequencyFormatter  frequencyFormatter1  headlineStyle �� 
TargetType �� Property �� Value  formTextBoxStyle(   "AllMyMusic_v3.Controls.FormTextBox  formLabelStyle&    AllMyMusic_v3.Controls.FormLabel  formLabelUOMStyle)   #AllMyMusic_v3.Controls.FormLabelUOM  formComboBoxStyle)   #AllMyMusic_v3.Controls.FormComboBox  formRatingStyle'   !AllMyMusic_v3.Controls.FormRating�� 5   
    5      �� 5      $+ %../ResourceDictionaries/MainSkin.xaml?�6%   �� 5      $3 -../ResourceDictionaries/MainGridSplitter.xaml?�6%   %P  &	        &	      &	 &     &	 9     &	 �     &	 �    &	 �    &	 �    &	 �    �� 5      6+     5      6*     5      6$   �� 5      " ��6*   .�5      ��  � ��$ Center=�66   �� 5       � ��$ 14(�6-   5      �� 5      "  6-   .�5      ��  � ��$ 0,3,0,0q�6+   �� 5       �@ 
LabelWidth$	 120Q�6/   �� 5        �@ LabelPadding$ 15,0,0,0q�61   �� 5!      ' �@ LabelHorizontalContentAlignment$
 Left=�6D   �� 5"       �@ TextBoxMargin$ 0,0,15,0q�62   5#      �� 5%      "  6+   .�5&      ��  � ��$ 0,3,0,0q�6+   �� 5'       �@ 
LabelWidth$	 120Q�6/   �� 5(       �@ LabelPadding$ 15,0,0,0q�61   �� 5)      ' �@ LabelHorizontalContentAlignment$
 Left=�6D   5*      �� 5,      "  6.   .�5-      ��  � ��$ 0,3,0,0q�6+   �� 5.       �@ 
LabelWidth$	 120Q�6/   �� 5/       �@ LabelPadding$ 15,0,0,0q�61   �� 50      ' �@ LabelHorizontalContentAlignment$
 Left=�6D   51      �� 53      "  6.   .�54      ��  � ��$ 0,3,0,0q�6+   �� 55       �@ 
LabelWidth$	 120Q�6/   �� 56       �@ LabelPadding$ 15,0,0,0q�61   �� 57      ' �@ LabelHorizontalContentAlignment$
 Left=�6D   �� 58       �@ ComboBoxWidth$	 100Q�62   59      �� 5;      "  6,   .�5<      ��  � ��$ 0,3,0,0q�6+   �� 5=       �@ 
LabelWidth$	 120Q�6/   �� 5>       �@ LabelPadding$ 15,0,0,0q�61   �� 5?      ' �@ LabelHorizontalContentAlignment$
 Left=�6D   5@      5B      .��5E      �� $
��Auto=�6   $
��Auto=�5F      .��5H   
   ��  �� Orientation$ Vertical=�6   
�������.+�5I      �� -   5        $��lableContributors��5I      #��[ 6-   $��0,10,0,0q�6   $
��Bold"�6�   ��6T   ��  �� Path$ ContributorsLabel�6�     5K      -   5          Name$ leadPerformer��5K   &   #��[ 6�   	   Label	 6=   �� $ LeadPerformerLabel�
   Text
 6g   �� $ Song.LeadPerformer� �� Mode$ TwoWay=�6�     5L      -   5        $ bandName��5L   &   #��[ 6�   	 68   �� $ 	BandLabel�
 6Y   �� $ Song.BandName�$ TwoWay=�6�     5M      -   5        $ bandSortName��5M   &   (   "AllMyMusic_v3.Controls.FormTextBox!   PreviewMouseLeftButtonDown$- 'bandSortName_PreviewMouseLeftButtonDown��6�   #��[ 6�   	 6<   �� $ BandSortLabel�
 6a   �� $ Song.BandSortName�$ TwoWay=�6�     5N      -   5        $ composerName��5N   &   #��[ 6�   	 6<   �� $ ComposerLabel�
 6a   �� $ Song.ComposerName�$ TwoWay=�6�     5O      -   5        $ conductorName��5O   &   #��[ 6�   	 6=   �� $ ConductorLabel�
 6c   �� $ Song.ConductorName�$ TwoWay=�6�   �� 5Q      -   5        $��lableAlbumInformation��5Q      #��[ 61   $��0,10,0,0q�6�   $
��Bold"�6�   ��6X   �� $ Album_InfoLabel�6�     5S      -   5        $ album��5S   &   #��[ 6�   	 65   �� $ 
AlbumLabel�
 6W   �� $ Song.AlbumName�$ TwoWay=�6�     5T      -	   5          Name$
 path��5T   $   #��[ 6w      Label 62   �� $ 	PathLabel�   Text 6S   �� $ Song.SongPath�6w   �� 5V      $ 
Horizontal=�6   .+�5W        -
   5          Name$ 
cbLanguage��5W   +   #��[ 6�      Label 6?   �� $ LanguageLabel�   ItemList 6d   �� $ 	Languages�   SelectedItem 6�   �� $ Song.Language�$ TwoWay=�6�     5X      -   5        $ 	cbCountry��5X   +   #��[ 6�    6>   �� $ CountryLabel� 6b   �� $ 	Countries� 6�   �� $ Song.Country�$ TwoWay=�6�   $	   AllMyMusic_v3.View.viewCountry	  5Y      $��20,0,0,0q�5Z      $��21��5[       � DataContext 5\      �� $ Country�5^      �� 5`      $ 
Horizontal=�6   .+�5a        -   5        $ 
albumGenre��5a   (   #��[ 6�      ContentWidth$	 100Q�6�    6<   �� $ AlbumGenreLabel� 6c   �� $ Song.AlbumGenre�6�     5b      -   5        $
 year��5b   *   #��[ 6�      TextBoxWidth$ 50Q�6�   	 68   �� $ 	YearLabel�
 6Y   �� $ 	Song.Year�$ TwoWay=�6�   5c        5d      -   5        $ variousArtists��5d   $   #��[ 6�    6<   �� $ ArtistTypeLabel� 6c   �� $ Song.ArtistType�6�   �� 5g      -   5        $��lableSongInformation��5g      #��[ 60   $��0,10,0,0q�6   $
��Bold"�6�   ��6W   �� $ Song_InfoLabel�6�     5i      -   5        $ 	songTitle��5i   &   #��[ 6�   	 69   �� $ SongTitleLabel�
 6_   �� $ Song.SongTitle�$ TwoWay=�y�5j   *   �� $ 
SingleSong� �� 	Converter# [    5k      -   5        $ filename��5k   $   #��[ 6�    66   �� $ FilenameLabel� 6[   �� $ Song.SongFilename�y�5l   )   �� $ 
SingleSong�# [  �� 5n      $ 
Horizontal=�6   
�������.+�5o        -   5        $ cbGenre��5o   +   #��[ 6�    6<   �� $ 
GenreLabel� 6^   �� $ Genres� 6   �� $ 
Song.Genre�$ TwoWay=�6�     5p      -   5          Name$ ratingSlider��5p   )   #��[ 6�      Label 6?   �� $ RatingLabel�   Rating 6b   �� $ Song.Rating�$ TwoWay=�6�   5q      �� 5s      $ 
Horizontal=�6   .+�5t        -   5        $ length��5t   (   #��[ 6   $	 100Q�6�    68   �� $ LengthLabel� 6[   �� $ Song.Duration�y�5u   ,   �� $ 
SingleSong�# [    5v      -   5        $ track��5v   *   #��[ 6�   $ 50Q�6�   	 69   �� $ 
TrackLabel�
 6[   �� $ 
Song.Track�$ TwoWay=�y�5w   ,   �� $ 
SingleSong�# [  5x        5z      -   5        $ songComment��5z   &   #��[ 6�   $��60��6�   	 6;   �� $ CommentLabel�
 6_   �� $ Song.Comment�$ TwoWay=�6�   �� 5}      -   5        $��lableTechnicalInformation��5}      #��[ 65   $
��Auto��6�   $��0,10,0,0q�6�   $
��Bold"�6�   ��6\   �� $ Technical_InfoLabel�6�     5      -   5          Name$ samplingRate��5   '   
   UOM$	 kHz��6�   #��[ 6�      Label 6=   �� $ SampleRateLabel�   Text 6d   �� $ Song.SampleRate�# [  �� ConverterParameter$
 ##.#��6�     5�      -   5        $ bitrate��5�   '   $
 kBps��6�   #��[ 6�    68   �� $ BitrateLabel� 6\   �� $ Song.Bitrate�# [ $	 ###��6�     5�      -   5        $ bitrateType��5�   $   #��[ 6�    69   �� $ BitrateTypeLabel� 6a   �� $ Song.BitrateType�  � ToolTip  5�      �� 5�      $ Vertical=�6!   .+�5�      �� .��6    CBR = Constant Bitrate68   �� 5�      .��6    VBR = Variable Bitrate68   5�      5�        5�      -   5        $ channels��5�   $   #��[ 6�   $	 100Q�6�    66   �� $ ChannelsLabel� 6[   �� $ Song.ChannelMode�y�5�   (   �� $ 
SingleSong�# [    5�      -   5        $ filesize��5�   $   #��[ 6�   $	 100Q�6�    66   �� $ FileSizeLabel� 6[   �� $ Song.FileSize�# [ y�5�   (   �� $ 
SingleSong�# [  �� 5�      -   5        $��lableWebsiteInformation��5�      #��[ 63   $
��Auto��6�   $��0,10,0,0q�6�   $
��Bold"�6�   ��6Z   �� $ Website_InfoLabel�6�     5�      -   5        $ websiteUser��5�   &   #��[ 6�   	 6;   �� $ Website_UserLabel�
 6d   �� $ Song.WebsiteUser�$ TwoWay=�6�     5�      -   5        $ websiteArtist��5�   &   #��[ 6�   	 6=   �� $ Website_ArtistLabel�
 6h   �� $ Song.WebsiteArtist�$ TwoWay=�6�   5�      5�      5�      