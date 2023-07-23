import { Pagination } from './pagination.model';

export class BaseFilter {
  sorting: string = 'id desc';
  pagination: Pagination = new Pagination();
}
