import { Directive, ElementRef, Input, OnChanges, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appScoreColor]',
  standalone: true
})
export class ScoreColorDirective implements OnChanges {

  @Input() appScoreColor = '';

  constructor(
    private element: ElementRef,
    private renderer: Renderer2
  ) {}

  ngOnChanges(): void {
    this.renderer.removeClass(this.element.nativeElement, 'score-low');
    this.renderer.removeClass(this.element.nativeElement, 'score-medium');
    this.renderer.removeClass(this.element.nativeElement, 'score-high');

    if (this.appScoreColor === 'High Match') {
      this.renderer.addClass(this.element.nativeElement, 'score-high');
    } else if (this.appScoreColor === 'Medium Match') {
      this.renderer.addClass(this.element.nativeElement, 'score-medium');
    } else {
      this.renderer.addClass(this.element.nativeElement, 'score-low');
    }
  }
}