   M S B A M L   `   `   `  ���� E  AAllMyMusic, Version=3.0.7.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AllMyMusic.ViewAllMyMusic.View  ;"clr-namespace:AllMyMusic.ViewModelAllMyMusic.ViewModel  9!clr-namespace:AllMyMusic.ControlsAllMyMusic.Controls  *    $AllMyMusic.View.viewPropertiesEditor   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ,vwclr-namespace:AllMyMusic.View     1vm"clr-namespace:AllMyMusic.ViewModel     9ammControls!clr-namespace:AllMyMusic.Controls       � 	Resources   �� MergedDictionaries �� Source   boolToVisibility-   'AllMyMusic.View.LongToFileSizeConverter  longToFilesizeConverter1(   "AllMyMusic.View.FrequencyFormatter  frequencyFormatter1  headlineStyle �� 
TargetType �� Property �� Value  formTextBoxStyle%   AllMyMusic.Controls.FormTextBox  formLabelStyle#   AllMyMusic.Controls.FormLabel  formLabelUOMStyle&    AllMyMusic.Controls.FormLabelUOM  formComboBoxStyle&    AllMyMusic.Controls.FormComboBox  formRatingStyle$   AllMyMusic.Controls.FormRating��  �� $. (../../ResourceDictionaries/MainSkin.xaml?��� $6 0../../ResourceDictionaries/MainGridSplitter.xaml?�%L  &	        &	      &	 
     &	      &	 L     &	 !    &	 �    &	 w    &	 G    ��     �� " ��.���  � ��$ Center=���  � ��$ 14(��� "  .���  � ��$ 0,3,0,0q���  �@ 
LabelWidth$	 120Q���  �@ LabelPadding$ 15,0,0,0q��� ' �@ LabelHorizontalContentAlignment$
 Left=���  �@ TextBoxMargin$ 0,0,15,0q��� "  .���  � ��$ 0,3,0,0q���  �@ 
LabelWidth$	 120Q���  �@ LabelPadding$ 15,0,0,0q��� ' �@ LabelHorizontalContentAlignment$
 Left=��� "  .���  � ��$ 0,3,0,0q���  �@ 
LabelWidth$	 120Q���  �@ LabelPadding$ 15,0,0,0q��� ' �@ LabelHorizontalContentAlignment$
 Left=��� "  .���  � ��$ 0,3,0,0q���  �@ 
LabelWidth$	 120Q���  �@ LabelPadding$ 15,0,0,0q��� ' �@ LabelHorizontalContentAlignment$
 Left=���  �@ ComboBoxWidth$	 100Q��� "  .���  � ��$ 0,3,0,0q���  �@ 
LabelWidth$	 120Q���  �@ LabelPadding$ 15,0,0,0q��� ' �@ LabelHorizontalContentAlignment$
 Left=�.���� $
��Auto=�$
��Auto=�.����  �� Orientation$ Vertical=�
�������.+��� -   $��lableContributors��#��[ $��0,10,0,0q�$
��Bold"�����  �� Path$ ContributorsLabel�  -     Name$ leadPerformer��#��[ 	   Label	 �� $ LeadPerformerLabel�
   Text
 �� $ Song.LeadPerformer� �� Mode$ TwoWay=�  -   $ bandName��#��[ 	 �� $ 	BandLabel�
 �� $ Song.BandName�$ TwoWay=�  -   $ bandSortName��%   AllMyMusic.Controls.FormTextBox!   PreviewMouseLeftButtonDown$- 'bandSortName_PreviewMouseLeftButtonDown��#��[ 	 �� $ BandSortLabel�
 �� $ Song.BandSortName�$ TwoWay=�  -   $ composerName��#��[ 	 �� $ ComposerLabel�
 �� $ Song.ComposerName�$ TwoWay=�  -   $ conductorName��#��[ 	 �� $ ConductorLabel�
 �� $ Song.ConductorName�$ TwoWay=��� -   $��lableAlbumInformation��#��[ $��0,10,0,0q�$
��Bold"����� $ Album_InfoLabel�  -   $ album��#��[ 	 �� $ 
AlbumLabel�
 �� $ Song.AlbumName�$ TwoWay=�  -	     Name$
 path��#��[    Label �� $ 	PathLabel�   Text �� $ Song.SongPath��� $ 
Horizontal=�.+�  -
     Name$ 
cbLanguage��#��[    Label �� $ LanguageLabel�   ItemList �� $ 	Languages�   SelectedItem �� $ Song.Language�$ TwoWay=�  -   $ 	cbCountry��#��[  �� $ CountryLabel� �� $ 	Countries� �� $ Song.Country�$ TwoWay=�!	   AllMyMusic.View.viewCountry	  $��20,0,0,0q�$��21�� � DataContext �� $ Country��� $ 
Horizontal=�.+�  -   $ 
albumGenre��#��[    ContentWidth$	 100Q� �� $ AlbumGenreLabel� �� $ Song.AlbumGenre�  -   $
 year��#��[    TextBoxWidth$ 50Q�	 �� $ 	YearLabel�
 �� $ 	Song.Year�$ TwoWay=�  -   $ variousArtists��#��[  �� $ ArtistTypeLabel� �� $ Song.ArtistType��� -   $��lableSongInformation��#��[ $��0,10,0,0q�$
��Bold"����� $ Song_InfoLabel�  -   $ 	songTitle��#��[ 	 �� $ SongTitleLabel�
 �� $ Song.SongTitle�$ TwoWay=�y��� $ 
SingleSong� �� 	Converter# [    -   $ filename��#��[  �� $ FilenameLabel� �� $ Song.SongFilename�y��� $ 
SingleSong�# [  �� $ 
Horizontal=�
�������.+�  -   $ cbGenre��#��[  �� $ 
GenreLabel� �� $ Genres� �� $ 
Song.Genre�$ TwoWay=�  -     Name$ ratingSlider��#��[    Label �� $ RatingLabel�   Rating �� $ Song.Rating�$ TwoWay=��� $ 
Horizontal=�.+�  -   $ length��#��[ $	 100Q� �� $ LengthLabel� �� $ Song.Duration�y��� $ 
SingleSong�# [    -   $ track��#��[ $ 50Q�	 �� $ 
TrackLabel�
 �� $ 
Song.Track�$ TwoWay=�y��� $ 
SingleSong�# [    -   $ songComment��#��[ $��60��	 �� $ CommentLabel�
 �� $ Song.Comment�$ TwoWay=��� -   $��lableTechnicalInformation��#��[ $
��Auto��$��0,10,0,0q�$
��Bold"����� $ Technical_InfoLabel�  -     Name$ samplingRate��
   UOM$	 kHz��#��[    Label �� $ SampleRateLabel�   Text �� $ Song.SampleRate�# [  �� ConverterParameter$
 ##.#��  -   $ bitrate��$
 kBps��#��[  �� $ BitrateLabel� �� $ Song.Bitrate�# [ $	 ###��  -   $ bitrateType��#��[  �� $ BitrateTypeLabel� �� $ Song.BitrateType�  � ToolTip  �� $ Vertical=�.+��� .��CBR = Constant Bitrate�� .��VBR = Variable Bitrate  -   $ channels��#��[ $	 100Q� �� $ ChannelsLabel� �� $ Song.ChannelMode�y��� $ 
SingleSong�# [    -   $ filesize��#��[ $	 100Q� �� $ FileSizeLabel� �� $ Song.FileSize�# [ y��� $ 
SingleSong�# [  �� -   $��lableWebsiteInformation��#��[ $
��Auto��$��0,10,0,0q�$
��Bold"����� $ Website_InfoLabel�  -   $ websiteUser��#��[ 	 �� $ Website_UserLabel�
 �� $ Song.WebsiteUser�$ TwoWay=�  -   $ websiteArtist��#��[ 	 �� $ Website_ArtistLabel�
 �� $ Song.WebsiteArtist�$ TwoWay=�