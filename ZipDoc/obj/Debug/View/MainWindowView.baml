   M S B A M L   `   `   `  ����A  =ZipDoc, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ZipDoc.ViewZipDoc.View  5   	        ZipDoc.View.MainWindowView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:ZipDoc.View     5   	   a ]DevExpress.Xpf.Core.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a?dxe5http://schemas.devexpress.com/winfx/2008/xaml/editors  5   	     � Title$  MainWindowView��5	   	   $��429.323��6    $	��750��61   $	��500��6=    � 	Resources 5
         TextBoxStyle �� 
TargetType �� Property �� Value  ButtonStyle��%�  &	        &	 �     �� 5   
   " ��6%   .�5      ��  � ��$ 30��6'   64   �� 5       �@��TextWrapping$
 Wrap=�6-   6<   �� 5       � ��$	 Top=�62   6@   �� 5       � ��$ 18.667(�6)   6:   5      �� 5   
   " ��6$   .�5      ��  � �� �	#FFFF52526?   �� 5       � �� �	#FFFF52526@   �� 5       � �� �	#FFF9F9F96?   �� 5       � ��$ 16(�6)   66   5      5      .��5      � ���	#FF303F9F.+�5   
   �� -   5         ��Name$ textBox��5      #��[  6#   $��30��6h   $��117,10,315,0q�6t    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   ��6I   �� *SearchPattern+6�   �� 5   
   -   5         ��Name$ searchButton��5      #��[ 6(   $��Search��6o   $��
0,10,196,0q�6�   $	��Top=�6�   $��30��6�   $��Right=�6�   $	��114��6�   ��6M   �� *SearchCommand+6�   � 5   
   ���#3F51B5$��0,58,0,0q�6$   .+�5      &   System.Windows.Controls.DataGrid  -   5          Name$ dataGrid��5      $��
10,0,10,39q�6)   	   SelectionMode$	 Single=�6z   ��6=   �� *Docs+
 �� SelectedItem
 6Z   �� *Selctd+5$      �� 5%      -   5        $ UploadButton��5%      #��[ 6+   $��Upload��6r   $
��Left=�6�   $��	135,0,0,9q�6�   $��Bottom=�6�   $	��120��6�   $��24��6�   ��6P   �� *UploadCommand+6�   �� 5&      -   5        $ RefreshButton��5&      #��[ 6,   $��Refresh��6t   $
��Left=�6�   $��
260,0,0,10q�6�   $	��117��6�   $��24��6�   $��Bottom=�6�   ��6Q   �� *RefreshCommand+6�   �� 5'      -   5        $ 
ExitButton��5'      #��[ 6)   $
��Exit��6n   $��Right=�6}   $��0,0,10,9q�6�   $��Bottom=�6�   $	��116��6�   ��6N   �� *ExitCommand+6�   �� 5(      -   5        $ DownloadButton��5(      #��[ 6-   $��Download��6v   $
��Left=�6�   $��	10,0,0,10q�6�   $��Bottom=�6�   $	��120��6�   $��24��6�   ��6R   �� *DownloadCommand+6�   � 5)      $
��Left=�6   $y�Hidden=�66    � Minimum$ 0Q�6J    � Maximum$	 100Q�6V   $��23��6�   $��382,307,0,0q�6�   $	��Top=�6�   $	��229��6�    � Value 6d   �� *CurrentProgressValue+6�   5*      �� 5+   
   -   5        $ LogoutButton��5+      #��[ 6'   $��Log out��6o   $��	0,10,10,0q�6�   $	��Top=�6�   $��30��6�   $��Right=�6�   $	��130��6�   ��6L   �� *LogOutCommand+6�   �� 5,   
   -	   5         ��Name$ label��5,      $��ZipDoc��6   $
��Left=�60   $��10,0,0,0q�6K   $	��Top=�6]   $��43��6u   $	��102��6�   $��Gadugi)�6�   $��26.667(�6�   
�������6�   5-      5.      