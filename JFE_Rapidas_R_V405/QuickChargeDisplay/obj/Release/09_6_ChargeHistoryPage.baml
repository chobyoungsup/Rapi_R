   M S B A M L   `   `   `  ���� M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  *    $QuickChargeDisplay.ChargeHistoryPage   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     2local clr-namespace:QuickChargeDisplay     -     N� Title$  ChargeHistoryPage��$	��800��$	��480�� � 	Resources  �� 
TargetType   ButtonTextBlock �� Property �� Value  GridViewHeaderTextBlock��%�  &	        &	 �     �� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��$
 Bold"���  � ��
 �������  � ��
 ���� ��  �@��TextAlignment$ Right=���  � |� .  �� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��$
 Bold"���  � ��
 �   ���  � ��
 ���� ��  �@��TextAlignment$ Right=���  � |� .   N� 
Background ��  �� ImageSource$8 2image\10_관리자화면\관리자기본배경.png��.���� .+��� -    ��Name$ chargeHistoryList�� �� Left$ 13.264��
	 �� Top$	 130.296��$��771.34��$��244.221������ 
 �� Path$

 List�   QuickChargeDisplay.QCDV   ChargeHistory �� Source# Z  �� View �� .P��� .���� #��[ NO �� CellTemplate �� .R�� $��80��$��30��.+���  �� TextAlignment$ Right=�$��15(�$��Right=�$��Center=����� $
 Number��� .���� #��[ Date �� .R��� $ Center=�$��15(�$	��150������ $

 Date��� .���� #��[ Time �� .R��� $ Center=�$��15(�$	��150������ $

 Time��� .���� #��[ Info �� .R��� $
 Left=�$��15(�$	��450������ $
 Details��� $ 464.396��$	 96.863��$��font\YoonYGO550 TT.ttf)�$
��Bold"�$��20(�$��34.296��$��66��$
��DATE���� -    ��Name$ cbHistoryDate��$ 538.092��$	 97.439��$	��200��$��30.009���� -    ��Name$ logOut��$'��!image/20191018_Add/back_12955.png��$	 719��$	 386.347��$��50��$��59.47���� -   $��TroubleHistory��$	 318��$	 0��$��80��$9��3image\10_관리자화면\아이콘/에러이력.png��$��79���� -   $��PassWord��$	 397��$	 0��$��80��$9��3image\10_관리자화면\아이콘/패스워드.png��$��79���� -   $��LogSampling��$	 720��$	 1��$��80��$9��3image\10_관리자화면\아이콘/로그추출.png��$��79���� -   $��SetupMaxium��$	 562��$	 1��$��78��$?��9image\10_관리자화면\아이콘/최대전류설정.png��$��77���� -	   $��TroubleDetails��$ 0��$	 1��$��80��$1��+image/RAPIDAS-R_Monitor/07/관리설정.png��$��78���� -
   $��SetupEquipment��$	 478��$	 0��$��80��$7��1image/RAPIDAS-R_Monitor/07/수전전력설정.png��$��82���� -   $��AdjustUnitPrice��$ 79��$	 0��$��80�� J� RenderTransformOrigin$ 0.5,0.5&�$1��+image/RAPIDAS-R_Monitor/07/일시정지.png��$��82���� -   $��
PowerMeter��$	 161��$	 0��$��80��$9��3image\10_관리자화면\아이콘/언어설정.png��$��83���� -   $��EquipmentInfo��$	 639��$	 1��$��80��$9��3image\10_관리자화면\아이콘/강제충전.png��$��80���� -   $��Langanglab1��$ 0��$	 54��$��管理設定��$��27��$��78�����	#FFF2E9E9 � ForceCursor .  $��Stretch=�$��Center=�$��Bottom=��� -   $��Langanglab2��$	 720��$	 55��$��	Log抽出�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -   $��Langanglab3��$ 79��$	 54��$��時間の設定�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -   $��Langanglab4��$	 161��$	 54��$��言語設定�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -   $��Langanglab5��$	 242��$	 54��$��充電履歴�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -   $��Langanglab6��$	 319��$	 53��$��エラー履歴�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -   $��Langanglab7��$	 398��$	 54��$��パスワード�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -   $��Langanglab8��$	 478��$	 54��$��受電電力設定�� .  ���	#FFF2E9E9$��27��$��Center=�$��Bottom=�$��81���� -   $��Langanglab9��$	 562��$	 54��$��電流設定�� .  ���	#FFF2E9E9$��27��$��Center=�$��Bottom=�$��78���� -   $��Langanglab10��$	 640��$	 55��$��Manual�� .  ���	#FFF2E9E9$��27��$��Center=�$��Bottom=�$��78���� -   $��Imgmanu��$	 562��$	 -1��$��81��$H��B/QuickChargeDisplay;component/image/10_관리자화면/Imgmanu.png��$	��238��$	��Top=� �� StretchDirection$
 Both=�