﻿using AutoMapper;
using Courseproject.Business.Exceptions;
using Courseproject.Business.Validation;
using Courseproject.Common.Dtos.Job;
using Courseproject.Common.Interfaces;
using Courseproject.Common.Model;
using FluentValidation;
//66

namespace Courseproject.Business.Services;

public class JobService : IJobService
{
    private IMapper Mapper { get; }
    private IGenericRepository<Job> JobRepository { get; }
    private JobCreateValidator JobCreateValidator { get; }
    private JobUpdateValidator JobUpdateValidator { get; }

    public JobService(IMapper mapper, IGenericRepository<Job> jobRepository,
        JobCreateValidator jobCreateValidator, JobUpdateValidator jobUpdateValidator)
    {
        Mapper = mapper;
        JobRepository = jobRepository;
        JobCreateValidator = jobCreateValidator;
        JobUpdateValidator = jobUpdateValidator;
    }

    public async Task<int> CreateJobAsync(JobCreate jobCreate)
    {
        //72
        await JobCreateValidator.ValidateAndThrowAsync(jobCreate);

        var entity = Mapper.Map<Job>(jobCreate);
        await JobRepository.InsertAsync(entity);
        await JobRepository.SaveChangesAsync();
        return entity.Id;
    }

    public async Task DeleteJobAsync(JobDelete jobDelete)
    {
        var entity = await JobRepository.GetByIdAsync(jobDelete.Id,
            (job) => job.Employees); //74 75
        //74 75
        if (entity == null)
            throw new JobNotFoundException(jobDelete.Id);
        //Don't want to delete addresses if we have an employee that uses this address
        if (entity.Employees.Count > 0)
            throw new DependentEmployeesExistException(entity.Employees);

        JobRepository.Delete(entity);
        await JobRepository.SaveChangesAsync();
    }

    public async Task<JobGet> GetJobAsync(int id)
    {
        var entity = await JobRepository.GetByIdAsync(id);
        //74 75
        if (entity == null)
            throw new JobNotFoundException(id);

        return Mapper.Map<JobGet>(entity);
    }

    public async Task<List<JobGet>> GetJobsAsync()
    {
        var entities = await JobRepository.GetAsync(null, null);
        return Mapper.Map<List<JobGet>>(entities);
    }

    public async Task UpdateJobAsync(JobUpdate jobUpdate)
    {
        //72
        await JobUpdateValidator.ValidateAndThrowAsync(jobUpdate);
        //73
        var existingJob = await JobRepository.GetByIdAsync(jobUpdate.Id); //db roundtrip
        if (existingJob == null)
            throw new JobNotFoundException(jobUpdate.Id);

        var entity = Mapper.Map<Job>(jobUpdate);
        JobRepository.Update(entity);
        await JobRepository.SaveChangesAsync();
    }
}
