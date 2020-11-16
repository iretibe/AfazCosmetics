import { Component, OnInit } from '@angular/core';
import { Meta } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Afaz Cosmetique';

  constructor(private meta: Meta){
    let i  = 0;
    let tim = setInterval(() => {

        let tag = this.meta.getTag('http-equiv=Content-Security-Policy');
        
        if (tag) {

          this.meta.removeTag('http-equiv=Content-Security-Policy');
          let content = tag.getAttribute('content');
          let str = 'connect-src ';debugger;
          let index = content.indexOf(str);
          content = content.slice(0, index + str.length) + "https://baseurl22/ https://baseurl23/ https://baseurl34/ " + content.slice(index + str.length);
            this.meta.updateTag({ 'http-equiv': 'Content-Security-Policy', content: content });
        } else {

          this.meta.addTag({ 'http-equiv': 'Content-Security-Policy', content: 'connect-src \'self\' https://baseurl1/ https://baseurl2/ https://baseurl3/;' });
        }

        if (i == 1) clearInterval(tim);
        i++;
    }, 1000);
  }

  ngOnInit(): void {}
}
