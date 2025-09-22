# 코드 설명
- 해당 코드들은 Unity ML-Agents에서 강화학습을 진행할 때 사용한 코드이며, 아래 github에서 제공하는 기본적인 mlagents 예제 파일들 중 Pyramid 예제 파일의 환경을 변형하여 제작하였습니다.
- 기존 코드 https://github.com/Unity-Technologies/ml-agents

1. Mobility.cs: 이동체(동적 장애물 움직임 표현 코드)
2. PyramidAgents.cs: Agent 코드
3. PyramidArea.cs: 시뮬레이션이 돌아가는 환경 코드(Agent와 Switch spawn 및 환경)
4. PyramidSwitch: Switch 상호작용 코드
5. SwitchManager: 강화학습을 위한 버튼(경유지) 개수 조정 관련 코드
