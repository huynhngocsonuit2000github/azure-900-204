param(
    [string]$ResourceGroupName = "long-running-gr",
    [string]$VMName = "main-running-vm-cost"
)

Write-Output "=== AutoStopIdleVM started ==="

try {
    Connect-AzAccount -Identity -ErrorAction Stop
    Select-AzSubscription -SubscriptionId "a0199a32-e233-4e1d-91de-6deb080a71c5"

    Write-Output "Checking VM..."
    $vm = Get-AzVM -ResourceGroupName $ResourceGroupName -Name $VMName -ErrorAction SilentlyContinue

    if (-not $vm) {
        Write-Output "❌ VM '$VMName' not found in resource group '$ResourceGroupName'."
        return
    }

    Write-Output "✅ Found VM: $($vm.Name)"
    $metrics = Get-AzMetric -ResourceId $vm.Id `
        -TimeGrain 00:05:00 `
        -StartTime (Get-Date).AddMinutes(-15) `
        -MetricName "Percentage CPU" `
        -ErrorAction Stop

    $avgCPU = ($metrics.Data | Where-Object { $_.Average -ne $null } | Measure-Object -Property Average -Average).Average
    Write-Output "Average CPU usage (last 15m): $avgCPU%"

    if ($avgCPU -lt 10) {
        Write-Output "CPU < 10%, stopping VM..."
        Stop-AzVM -Name $VMName -ResourceGroupName $ResourceGroupName -Force
        Write-Output "✅ VM stopped successfully!"
    } else {
        Write-Output "VM active, skipping stop."
    }

} catch {
    Write-Output "❌ Error caught: $($_.Exception.Message)"
    throw
}
