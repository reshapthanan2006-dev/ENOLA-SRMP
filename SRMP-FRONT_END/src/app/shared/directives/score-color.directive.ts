import { Directive, ElementRef, Input, OnChanges, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appScoreColor]',
  standalone: true
})
export class ScoreColorDirective implements OnChanges {

  @Input({ required: true }) appScoreColor!: number;

  constructor(
    private element: ElementRef,
    private renderer: Renderer2
  ) { }

  ngOnChanges(): void {

    for (const band of ['strong', 'good', 'moderate', 'low']) {
      this.renderer.removeClass(this.element.nativeElement, band);
    }

    let band: string;

    if (this.appScoreColor >= 80) {
      band = 'strong';
    } else if (this.appScoreColor >= 60) {
      band = 'good';
    } else if (this.appScoreColor >= 40) {
      band = 'moderate';
    } else {
      band = 'low';
    }

    this.renderer.addClass(this.element.nativeElement, band);
  }

}
