import { Directive, ElementRef, Input, OnChanges, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appHighlight]',
  standalone: true
})
export class HighlightDirective implements OnChanges {

  @Input() appHighlight = false;

  constructor(
    private element: ElementRef,
    private renderer: Renderer2
  ) { }

  ngOnChanges(): void {

    if (this.appHighlight) {
      this.renderer.setStyle(
        this.element.nativeElement,
        'background-color',
        '#fef3c7'
      );
    } else {
      this.renderer.removeStyle(
        this.element.nativeElement,
        'background-color'
      );
    }
  }

}