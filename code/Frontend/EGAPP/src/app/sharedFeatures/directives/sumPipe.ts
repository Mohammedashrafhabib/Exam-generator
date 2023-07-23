import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'sum',
})
export class SumPipe implements PipeTransform {
  transform(
    items: any[],
    attr: any,
    label?: any,
    secondLabel?: any
  ): any {
    return items
      .filter(c =>
        label
          ? c.label === label
          : secondLabel
          ? c.secondLabel === secondLabel
          : true
      )
      .reduce((a, b) => a + b[attr], 0);
  }
}
