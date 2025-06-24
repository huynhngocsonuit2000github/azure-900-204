# 1. Basic application

- [WIP]: Expose your Web API via a Public IP + Load Balancer, with the ability to scale to more VMs.

Architecture

<!--

Client (Internet)
       ↓
[Public IP] (Load Balancer)
       ↓
  Backend Pool (VM1, VM2...)
       ↓
   Web API on Port 80

 -->

1. Create simple VM, running Nginx Webapp
2. Create Public Load Balancer
3. Create Backend Pool
4. Create Health Probe
5. Add Load Balancing Rule
6. Test It

# 2. Enhance

- 2.1. Add SSL (HTTPS)
- 2.2. Test with multiple VMs
- 2.3. Switch to Internal Load Balancer
- 2.4. Add autoscaling for VM pool
