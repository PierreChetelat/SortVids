   M S B A M L   `   `   `  ����  
SortMyVids     SortMyVids.ResearchControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      �   � ColumnDefinitions  5	   
   �� 5
      $��3* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��60 �6   �� 5      $��60 �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��50 �6   .+�5   
   �� $ ��Dossier source des films :��6   $	��Top=�65   �� 5   
   -   5         ��Name$ uiFolderSrc��5      $"��Dossier des films non triés��6'   {�.  $��25��6\   $��Bottom=�6h   $��5,0,5,5q�6�   �� 5   
   -   5         ��Name$ uiButtonSrc��5      $��1��6&   $��Modifier...��66   $��Bottom=�6L   $��25��6�   $��70��6�   $��5q�6�   �� 5   
   $��Dossier de destination :��6   $	��Top=�63   $��1��6K   �� 5   
   -   5        $ uiFolderDest��5      $��1��6(   $��Dossier des films triés��65   {�.  $��25��6f   $��Bottom=�6r   $��5,0,5,5q�6�   �� 5   
   -   5        $ uiButtonDest��5      $��1��6'   $��1��67   $��Modifier...��6D   $��Bottom=�6Z   $��25��6�   $��70��6�   $��5q�6�   �� 5   
   -   5         ��Name$ uiListMovie��5      $��2��6'   $��2��64   $��5,25,5,5q�6E   �� 5   
   -   5         ��Name$ uiLabelNBVideo��5      $��Vidéos trouvés : 0��6(   $	��Top=�6E   $��Center=�6]   $��2��6z   � 5   
   -   5         �Name$ uiProgressBarAnalyse��5      $��4��64   $��15��6A   $��Bottom=�6M   $��5q�6h   �� 5    
   -   5        $ uiButtonLaunchAnalysis��5       $��4��61   $��Center=�6>   $	��Top=�6[   $��25��6s   $��60��6   $��Analyser��6�   5!      5"      