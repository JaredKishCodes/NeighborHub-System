import { DomainUser } from "./domainUser.model";

export interface Item {
  id: number;
  name: string;
  description: string;
  category: string;
  itemStatus: ItemStatus;
  imageUrl?: string; // optional
  createdAt: string;
  lastUpdatedAt: string;

  ownerId: number;
  owner?: DomainUser;
}

export enum ItemStatus {
  Available = 0,
  Requested = 1,
  Borrowed = 2
}