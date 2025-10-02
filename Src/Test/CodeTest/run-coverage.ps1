# Code Coverage Test Script for CodeTest
# This script runs tests with code coverage and generates reports

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Code Coverage Test Runner" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Set error action preference
$ErrorActionPreference = "Stop"

# Define paths
$ProjectPath = $PSScriptRoot
$TestResultsPath = Join-Path $ProjectPath "TestResults"
$CoverageReportPath = Join-Path $TestResultsPath "CoverageReport"

# Clean up previous test results
Write-Host "Cleaning up previous test results..." -ForegroundColor Yellow
if (Test-Path $TestResultsPath) {
    Remove-Item -Path $TestResultsPath -Recurse -Force
}
New-Item -ItemType Directory -Path $TestResultsPath -Force | Out-Null
Write-Host "✓ Cleanup completed" -ForegroundColor Green
Write-Host ""

# Run tests with coverage
Write-Host "Running tests with code coverage..." -ForegroundColor Yellow
try {
    dotnet test `
        --configuration Debug `
        --settings coverlet.runsettings `
        --collect:"XPlat Code Coverage" `
        --results-directory $TestResultsPath `
        --logger "console;verbosity=detailed"

    if ($LASTEXITCODE -ne 0) {
        Write-Host "✗ Tests failed!" -ForegroundColor Red
        exit $LASTEXITCODE
    }
    Write-Host "✓ Tests completed successfully" -ForegroundColor Green
} catch {
    Write-Host "✗ Error running tests: $_" -ForegroundColor Red
    exit 1
}
Write-Host ""

# Find coverage file
Write-Host "Locating coverage file..." -ForegroundColor Yellow
$CoverageFile = Get-ChildItem -Path $TestResultsPath -Filter "coverage.cobertura.xml" -Recurse | Select-Object -First 1

if (-not $CoverageFile) {
    Write-Host "✗ Coverage file not found!" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Coverage file found: $($CoverageFile.FullName)" -ForegroundColor Green
Write-Host ""

# Install ReportGenerator if not already installed
Write-Host "Checking ReportGenerator tool..." -ForegroundColor Yellow
$ReportGeneratorInstalled = dotnet tool list --global | Select-String "dotnet-reportgenerator-globaltool"
if (-not $ReportGeneratorInstalled) {
    Write-Host "Installing ReportGenerator..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-reportgenerator-globaltool
    Write-Host "✓ ReportGenerator installed" -ForegroundColor Green
} else {
    Write-Host "✓ ReportGenerator already installed" -ForegroundColor Green
}
Write-Host ""

# Generate HTML report
Write-Host "Generating HTML coverage report..." -ForegroundColor Yellow
try {
    reportgenerator `
        -reports:$($CoverageFile.FullName) `
        -targetdir:$CoverageReportPath `
        -reporttypes:"Html;HtmlSummary;Badges;TextSummary;MarkdownSummary" `
        -verbosity:Info `
        -assemblyfilters:"+Container"

    Write-Host "✓ Coverage report generated" -ForegroundColor Green
} catch {
    Write-Host "✗ Error generating report: $_" -ForegroundColor Red
    exit 1
}
Write-Host ""

# Display summary
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Coverage Summary" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

$SummaryFile = Join-Path $CoverageReportPath "Summary.txt"
if (Test-Path $SummaryFile) {
    Get-Content $SummaryFile | Write-Host
}
Write-Host ""

# Open report in browser
$ReportIndexFile = Join-Path $CoverageReportPath "index.html"
Write-Host "Coverage report location:" -ForegroundColor Cyan
Write-Host $ReportIndexFile -ForegroundColor White
Write-Host ""
Write-Host "Opening coverage report in browser..." -ForegroundColor Yellow
Start-Process $ReportIndexFile

Write-Host "✓ Coverage analysis completed!" -ForegroundColor Green
