   M S B A M L   `   `   `  ����R  NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R8clr-namespace:System.ComponentModel;assembly=WindowsBaseSystem.ComponentModel  5   	    	shiftgame   shiftgame.HighScoreWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   >d2http://schemas.microsoft.com/expression/blend/2008     5   	   Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6F   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml      5   	   Hscm8clr-namespace:System.ComponentModel;assembly=WindowsBase      5   	   -   5          � Title$  Ranglista - Shift Game��5   	   $	��351��6(   $	��300��65   
���   � � Icon$" Images/shiftgame-logo1x1.png��6k   .��5      �  � 	Resources 5   
      cvs �� Source �� ElementName �� Path �� SortDescriptions+   %System.ComponentModel.SortDescription   PropertyName �� 
TargetType	 �� 	Resources
 �� Color��%�  &	        '�� �     �� 5	       6/   �� $ comboboxLevelHighscores��$ SelectedItem.Results� 5
        5      $ EllapsedSeconds��6*     5      $ Steps��6*   5      �� 5      " ��6   	 5      ���� 5      $
 Black��6X   (��       6&   * SystemColors.HighlightBrushKey+)6X   �� 5      $

 Gray��6i   (��       6&   *1/SystemColors.InactiveSelectionHighlightBrushKey+)6i   5      5      5      .+�5   
   �� -   5         ��Name$ listboxLevelHighscores��5      $
��Left=�6~   $	��123��6�   $��
10,176,0,0q�6�   $	��Top=�6�   $	��256��6�   ���	#FF212121���	#FF2E2E2E � DataContext 62   �� ��6J   ��  �� Source# [  ��5      �� 5      .R�5      � $��33��6C   .+�5      �� $��	0,0,119,0q�6@   
�������$��16(�6f   $��Arial)�6t   $��SemiBold"�6�   ��6    �� *Player.Name+6�   �� 5      $��
127,0,59,0q�6D   
�������$��16(�6k   $��Arial)�6y   $��SemiBold"�6�   ��6    �� *EllapsedSeconds+6�   �� 5      $��	187,0,0,0q�6:   
�������$��16(�6`   $��Arial)�6n   $��SemiBold"�6�   ��6    �� *Steps+6�   5      5      5!      �� 5"   
   $��	Játékos��6   $��Center=�6"   $��10,148,206,0q�6?   $	��Top=�6U   $��16(�6m   $��Arial)�6{   $��SemiBold"�6�   
�������$��76��6�    J� IsHitTestVisible .  6�   �� 5#   
   $��Pálya��6   $
��Left=�6    $��
10,105,0,0q�6;   $	��Top=�6O   
�������$��16(�6z   $��Arial)�6�   $��SemiBold"�6�   �� 5$   
   -   5         ��Name$ 	imageLogo��5$      $��Center=�6#   $	��100��6@   $��	74,9,74,0q�6M   $	��Top=�6`   $	��144��6x   $��Images/shiftgame-logo.png��6�   �� 5%   
   -   5         ��Name$ comboboxLevelHighscores��5%       �� ItemStringFormat$ Results.Level��64   $
��Left=�6U   $��121,111,0,0q�6p   $	��Top=�6�   $	��145��6�   
������� �� SelectedIndex$ 0��6�   �� 5&   
   $��	Idő (mp)��6   $��Center=�6#   $��121,148,92,0q�6@   $	��Top=�6V   $��16(�6n   $��Arial)�6|   $��SemiBold"�6�   
�������$��79��6�    .  6�   �� 5'   
   $��Lépés��6   $��Center=�6    $��205,148,26,0q�6=   $	��Top=�6S   $��16(�6k   $��Arial)�6y   $��SemiBold"�6�   
�������$��61��6�    .  6�   5(      5)      