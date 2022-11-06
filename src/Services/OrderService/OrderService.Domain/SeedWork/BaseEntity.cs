﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.SeedWork
{
    public abstract class BaseEntity
    {
        public virtual Guid Id { get; set; }
        public DateTime CreateDate { get; set; }


        int? _requestedHashCode;

        private List<INotification> domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => domainEvents?.AsReadOnly();


        public void AddDomainEvent (INotification eventItem)
        {
            domainEvents = domainEvents ?? new List<INotification>();
            domainEvents.Add(eventItem);
        }
        public void RemoveDomainEvent(INotification eventItem)
        {
            domainEvents?.Remove(eventItem);
        }
        public void ClearDomainEvent(INotification eventItem)
        {
            domainEvents?.Clear();
        }
        public bool IsTransient()
        {
            return Id == default;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is BaseEntity))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (GetType() != obj.GetType())
                return false;

            BaseEntity item = (BaseEntity)obj;

            if (item.IsTransient() || IsTransient())
                return false;
            else
                return item.Id == Id;
            
        }
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = Id.GetHashCode() ^ 31;

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }
        public static bool operator ==(BaseEntity left,BaseEntity right)
        {
            if (Equals(left, null))
                return Equals(right, null) ? true : false;
            else
                return left.Equals(right);
        }
        public static bool operator !=(BaseEntity left, BaseEntity right)
        {
            return !(left == right);
        }
    }

}

