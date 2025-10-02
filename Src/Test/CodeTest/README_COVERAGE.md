# 코드 커버리지 가이드

## 📋 개요

CodeTest 프로젝트는 코드 커버리지를 자동으로 수집하여, 테스트를 거치지 않는 코드가 없도록 보장합니다.

## 🚀 빠른 시작

### Windows (PowerShell)
```powershell
.\run-coverage.ps1
```

### Linux/macOS (Bash)
```bash
chmod +x run-coverage.sh
./run-coverage.sh
```

## 📊 커버리지 보고서

스크립트를 실행하면 다음이 자동으로 수행됩니다:

1. ✅ 기존 테스트 결과 정리
2. ✅ 코드 커버리지를 수집하며 테스트 실행
3. ✅ HTML 커버리지 보고서 생성
4. ✅ 브라우저에서 보고서 자동 열기

### 보고서 위치

```
Src/Test/CodeTest/TestResults/CoverageReport/index.html
```

## 📈 커버리지 설정

### 프로젝트 설정 (CodeTest.csproj)

```xml
<!-- Code Coverage Settings -->
<CollectCoverage>true</CollectCoverage>
<CoverletOutputFormat>cobertura,json</CoverletOutputFormat>
<CoverletOutput>./TestResults/</CoverletOutput>
<Threshold>0</Threshold>
<ThresholdType>line,branch,method</ThresholdType>
<ThresholdStat>total</ThresholdStat>
<ExcludeByFile>**/Migrations/*.cs</ExcludeByFile>
<Include>[Container]*</Include>
```

### 커버리지 임계값 조정

현재 임계값은 `0%`로 설정되어 있습니다. 프로젝트가 성숙해지면 다음과 같이 조정할 수 있습니다:

```xml
<Threshold>80</Threshold>
```

이렇게 설정하면 라인, 브랜치, 메서드 커버리지가 80% 미만일 때 빌드가 실패합니다.

## 🎯 커버리지 목표

| 지표 | 목표 |
|------|------|
| **Line Coverage** | 80% 이상 |
| **Branch Coverage** | 80% 이상 |
| **Method Coverage** | 80% 이상 |

## 📝 수동 실행

스크립트를 사용하지 않고 수동으로 실행하려면:

```bash
# 테스트 실행 및 커버리지 수집
dotnet test --settings coverlet.runsettings --collect:"XPlat Code Coverage"

# ReportGenerator 설치 (한 번만)
dotnet tool install --global dotnet-reportgenerator-globaltool

# HTML 보고서 생성
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"TestResults/CoverageReport" -reporttypes:"Html;HtmlSummary"
```

## 🔍 커버리지 보고서 이해하기

### 보고서 구성 요소

- **Line Coverage**: 실행된 코드 라인의 비율
- **Branch Coverage**: 실행된 분기(if, switch 등)의 비율
- **Method Coverage**: 호출된 메서드의 비율

### 색상 코드

- 🟢 **녹색**: 높은 커버리지 (80% 이상)
- 🟡 **노란색**: 중간 커버리지 (60-79%)
- 🔴 **빨간색**: 낮은 커버리지 (60% 미만)

## 🛠️ CI/CD 통합

### GitHub Actions 예시

```yaml
- name: Run tests with coverage
  run: |
    cd Src/Test/CodeTest
    dotnet test --settings coverlet.runsettings --collect:"XPlat Code Coverage"

- name: Generate coverage report
  run: |
    dotnet tool install --global dotnet-reportgenerator-globaltool
    reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:"Html;Cobertura"

- name: Upload coverage report
  uses: actions/upload-artifact@v3
  with:
    name: coverage-report
    path: coverage-report
```

## 📚 추가 리소스

- [Coverlet 문서](https://github.com/coverlet-coverage/coverlet)
- [ReportGenerator 문서](https://github.com/danielpalme/ReportGenerator)
- [.NET 테스트 커버리지 가이드](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-code-coverage)