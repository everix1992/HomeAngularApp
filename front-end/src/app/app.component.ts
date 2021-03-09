import { Component, OnInit } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public input?: string;
  public resultText: string = "";
  public selectedSolution?: string;
  public solutionNames: string[] = [];

  constructor(private http: HttpClient) {

  }

  async ngOnInit() {
    const apiUrl = new URL('/api/v1/AdventOfCodeSolutions', environment.apiBaseUrl);
    const results = await this.http.get<string[]>(apiUrl.href).toPromise();

    if (results && results.length > 0) {
      this.solutionNames = results;
      this.selectedSolution = this.solutionNames[0];
    }
  }

  clearButtonClicked() {
    this.input = undefined;

    if (this.solutionNames && this.solutionNames.length > 0) {
      this.selectedSolution = this.solutionNames[0];
    }

    this.resultText = "";
  }

  async runButtonClicked() {
    const apiUrl = new URL('/api/v1/AdventOfCodeSolutions/GetSolution', environment.apiBaseUrl);

    // TODO: Strongly type this
    const requestBody = {
      Input: this.input,
      SolutionName: this.selectedSolution
    };

    const result = await this.http.post<string>(apiUrl.href, requestBody).toPromise();
    this.resultText = result;
  }
}
