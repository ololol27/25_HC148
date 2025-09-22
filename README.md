# 2025 한이음 드림업 공모전

## 1. 프로젝트 개요
### 1-1. 프로젝트 소개
- 프로젝트 명: 강화학습/디지털트윈 기반 모바일로봇 군집제어 및 최적화 플랫폼
- 프로젝트 정의: AI 기반 강화학습과 디지털 트윈 기술을 결합하여 복잡한 환경에 스스로 적응하는 모바일 로봇의 자율 제어 기술
### 1-2. 개발 배경 및 필요성
아마존이나 쿠팡같은 물류센터와 반도체 제조 공장에서 수많은 자율 이동 로봇을 운영하며 효율을 추구하고 있습니다. 그러나 기존 로봇 제어 방식은 미리 정해진 규칙에 의존해 유연성이 부족하며, 돌발 상황 발생 시 전체 시스템에 영향을 줄 수 있습니다. 이러한 한계를 극복하기 위해, 본 프로젝트는 로봇이 스스로 환경을 인식하고 판단하여 행동하는 피지컬 AI(Physical AI)를 도입해 실제 산업 현장의 문제에 대한 실용적인 해결책을 제시하고자 합니다.
### 1-3. 프로젝트 특장점
- 디지털 트윈을 AI 기반 강화학습의 훈련 환경으로 활용하여 로봇이 시행착오를 겪는 과정 개선
- 실제 환경에 적용하여 향상된 성능 확인
- 정적 장애물뿐만 아니라 동적 장애물에 대한 대응 능력 구현
- 높은 정밀도를 요구하는 산업 현장에 즉시 적용 가능성 확보
- 물류창고, 병원 등 다양한 가상 환경을 구축하여 기술 적용 가능성 확장
### 1-4. 주요 기능
- Unity 환경에서 AI 기반 강화학습 모델 훈련
-	동적 장애물 회피 및 경로 최적화
-	가상 환경에서 학습된 정책을 실제 로봇에 적용
-	실시간 센서 데이터 기반 자율 주행
-	다중 에이전트 협력 전략 학습
### 1-5. 기대 효과 및 활용 분야
-	기대효과: 기존 제어 방식 대비 실시간 적응 및 확장성 우수한 자율 제어 구현, 디지털 트윈을 활용한 실험 효율성 극대화 및 비용 절감
-	활용 분야: 스마트 물류 및 창고 관리 시스템, 반도체 제조 공장 자동화, 스마트 도서관 시스템, 카페 및 식당용 자율 서빙 로봇 시스템, 교육용 시뮬레이션 및 학습 플랫폼
### 1-6. 기술 스택
-	소프트웨어: Unity ML-Agents, ROS, PyTorch, SLAM, Visual Studio
-	언어: C#, Python, C++(아두이노)
-	하드웨어: TurtleBot3 Burger, RC카
-	센서: LiDAR, IMU, 적외선 센서
## 2. 팀원 소개
|여기에 사진|여기에 사진|![Image](https://github.com/user-attachments/assets/41d16783-946b-449f-a498-02521e9b22e9)|![Image](https://github.com/user-attachments/assets/c22e5d5f-d5b0-48d2-809f-a3a9d781fa34)|여기에 사진|
|:---:|:---:|:---:|:---:|:---:|
|**이승은(팀장)**|**박경현**|**원예림**|**이소현**|**조한별(멘토)**|
|ML-Agents 환경 구성 <br/>& 성능 검증|physical AI 시뮬레이션 구현 (Turtlebot) <br/>& Digital Twin 환경 구축|physical AI 시뮬레이션 구현 (Turtlebot) <br/>&장애물 이동체 관리(Arduino)|ML-Agents 환경 구성 및 동작 구현 <br/>& 강화학습|피드백 및 방향성 제시|

## 3. 시스템 구성도
- 프로젝트 개요도
<img width="478" height="467" alt="Image" src="https://github.com/user-attachments/assets/a1a64491-bcd1-4b28-95bd-355a8ca349b4" />

- H/W와 S/W 흐름도
<img width="1920" height="1080" alt="Image" src="https://github.com/user-attachments/assets/326dfd8a-df3d-4c7e-ad34-3784cbe6b0b4" />

## 4. 작품 소개 영상
[![2025년 한이음 드림업 공모전 시연 동영상 - 강화학습 기반 다수 모바일 로봇 군집제어 및 최적화 플랫폼](https://i.ytimg.com/vi/_nOzjau9hys/hq720.jpg)](https://www.youtube.com/watch?v=_nOzjau9hys)

## 5. 핵심 소스코드
- 전체 소스코드 위치: Assets/ML-Agents/Examples/Pyramids/Scripts
- 소스코드 이름 설명: Assets/ML-Agents/Examples/Pyramids/Scripts/CODES.md
- 참고한 코드 및 환경: https://github.com/Unity-Technologies/ml-agents  (Pyramids 예제)
- 강화학습에 사용된 yaml 설정 파일과 최종 결과 파일은 원래 위의 mlagents GitHub 저장소의 config 및 result 디렉토리에 위치해 있었습니다. 그러나 원본 저장소와의 연결 문제로 로컬 파일이 덮어씌워지는 문제가 발생하여, 안전한 공유를 위해 해당 파일들을 Notion에 별도로 업로드하였습니다.
  https://www.notion.so/yaml-result-27639fe749d780c88001d9d5b5bc2269?source=copy_link
  
### 5-1. 보상값 체제 구현 코드
- 강화학습 에이전트의 두뇌 역할을 하며, 환경으로부터 받은 보상에 따라 행동을 학습하는 메서드입니다.
```csharp
public override void OnActionReceived(ActionBuffers actionBuffers)
{
  AddReward(-1f / MaxStep); // step penalty
  stepCount++;
  MoveAgent(actionBuffers.DiscreteActions);
}

public override void OnEpisodeBegin()
{
  episodeCounter++;
  episodeStartTime = Time.realtimeSinceStartup;
  m_MyArea.CleanPyramidArea();
  m_AgentRb.velocity = Vector3.zero;
  m_AgentRb.angularVelocity = Vector3.zero;
  transform.position = initialPos;
  transform.rotation = initialRot;
  foreach (var mob in linkedMobilities) if (mob != null) mob.ResetMobility();
  if (!switchManager) switchManager = FindFirstObjectByType<SwitchManager>();
  switchManager.StartSequence();
  stepCount = 0;
  visitedSwitches.Clear();
}

void OnCollisionEnter(Collision collision)
{
  if (collision.gameObject.CompareTag("wall"))
  {
    AddReward(-2f); // 벽 충돌
  }
  if (collision.gameObject.CompareTag("mobility"))
  {
    AddReward(-3f); // Mobility 충돌
  }
  if (collision.gameObject.CompareTag("goal"))
  {
    AddReward(goalReward); // Goal 보상
    LogEpisodeSwitches(true);
    foreach (var mob in linkedMobilities) if (mob != null) mob.ResetMobility();
    EndEpisode();
  }
}
```
### 5-2. 동적 장애물(RC카) 코드
- Agent의 경로를 방해하는 동적 장애물(RC카)의 움직임을 구현하는 메서드입니다.
``` csharp
void FixedUpdate()
{
  if (pathPoints == null || pathPoints.Length == 0) return;
  Vector3 target = pathPoints[currentPoint];
  Vector3 direction = (target - transform.position).normalized;
  rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
  if (Vector3.Distance(transform.position, target) < reachThreshold)
  {
    currentPoint = (currentPoint + 1) % pathPoints.Length;
  }
}

public void ResetMobility()
{
  rb.velocity = Vector3.zero;
  rb.angularVelocity = Vector3.zero;
  transform.position = initialPos;
  transform.rotation = initialRot;
  currentPoint = 0;
  BuildPath(initialPos);
}
```
### 5-3. 스위치(경유지 및 목적지) 구현 코드
- Agent가 밟아야 하는 스위치의 상호작용을 정의하는 메서드입니다.
``` csharp
void OnCollisionEnter(Collision other)
{
  if (m_State) return;
  if (!other.gameObject.CompareTag("agent")) return;
  myButton.GetComponent<Renderer>().material = onMaterial;
  m_State = true;
  tag = "switchOn";
  var ag = other.gameObject.GetComponent<PyramidAgent>();
  if (ag) ag.RecordSwitch(transform.position);
  if (!manager) manager = FindFirstObjectByType<SwitchManager>();
  manager.OnSwitchPressed(this);
}
```
