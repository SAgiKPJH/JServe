#!/bin/bash
# Code Coverage Test Script for CodeTest (Linux/macOS)
# This script runs tests with code coverage and generates reports

echo "========================================"
echo "Code Coverage Test Runner"
echo "========================================"
echo ""

# Set error handling
set -e

# Define paths
PROJECT_PATH="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
TEST_RESULTS_PATH="$PROJECT_PATH/TestResults"
COVERAGE_REPORT_PATH="$TEST_RESULTS_PATH/CoverageReport"

# Clean up previous test results
echo "Cleaning up previous test results..."
if [ -d "$TEST_RESULTS_PATH" ]; then
    rm -rf "$TEST_RESULTS_PATH"
fi
mkdir -p "$TEST_RESULTS_PATH"
echo "✓ Cleanup completed"
echo ""

# Run tests with coverage
echo "Running tests with code coverage..."
dotnet test \
    --configuration Debug \
    --settings coverlet.runsettings \
    --collect:"XPlat Code Coverage" \
    --results-directory "$TEST_RESULTS_PATH" \
    --logger "console;verbosity=detailed"

echo "✓ Tests completed successfully"
echo ""

# Find coverage file
echo "Locating coverage file..."
COVERAGE_FILE=$(find "$TEST_RESULTS_PATH" -name "coverage.cobertura.xml" | head -n 1)

if [ -z "$COVERAGE_FILE" ]; then
    echo "✗ Coverage file not found!"
    exit 1
fi
echo "✓ Coverage file found: $COVERAGE_FILE"
echo ""

# Install ReportGenerator if not already installed
echo "Checking ReportGenerator tool..."
if ! dotnet tool list --global | grep -q "dotnet-reportgenerator-globaltool"; then
    echo "Installing ReportGenerator..."
    dotnet tool install --global dotnet-reportgenerator-globaltool
    echo "✓ ReportGenerator installed"
else
    echo "✓ ReportGenerator already installed"
fi
echo ""

# Generate HTML report
echo "Generating HTML coverage report..."
reportgenerator \
    -reports:"$COVERAGE_FILE" \
    -targetdir:"$COVERAGE_REPORT_PATH" \
    -reporttypes:"Html;HtmlSummary;Badges;TextSummary" \
    -verbosity:Info

echo "✓ Coverage report generated"
echo ""

# Display summary
echo "========================================"
echo "Coverage Summary"
echo "========================================"

SUMMARY_FILE="$COVERAGE_REPORT_PATH/Summary.txt"
if [ -f "$SUMMARY_FILE" ]; then
    cat "$SUMMARY_FILE"
fi
echo ""

# Report location
REPORT_INDEX="$COVERAGE_REPORT_PATH/index.html"
echo "Coverage report location:"
echo "$REPORT_INDEX"
echo ""

# Try to open report in browser (varies by OS)
if command -v xdg-open > /dev/null; then
    xdg-open "$REPORT_INDEX" 2>/dev/null || true
elif command -v open > /dev/null; then
    open "$REPORT_INDEX" 2>/dev/null || true
fi

echo "✓ Coverage analysis completed!"
